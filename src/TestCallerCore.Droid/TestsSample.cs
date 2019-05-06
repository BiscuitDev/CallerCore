using System;
using NUnit.Framework;
using CallerCore.MainCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace TestCallerCore.Droid
{
    [TestFixture]
    public class TestsSample : BaseTestFixture
    {


        [SetUp]
        public void Setup()
        {
            Console.WriteLine("setup");
        }


        [TearDown]
        public void Tear()
        {
            Console.WriteLine("tear");
        }

        [Test]
        [TestFixtureSetUp]
        public void RegisterFunctions()
        {

        }
        [Test]
        public void Pass()
        {
            Console.WriteLine("test1");
            Assert.True(true);
        }

        [Test]
        public void TestCallFunction()
        {
            string ret = null;
            ret = vlr.mmc.CallFunction<string>(FunctionTypedTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = (string)vlr.mmc.CallFunction(FunctionTypedTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = vlr.mmc.CallFunction<string>(FunctionTypedTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = (string)vlr.mmc.CallFunction(FunctionTypedTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = vlr.mmc.CallFunction<string>(FunctionRegular.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = (string)vlr.mmc.CallFunction(FunctionRegular.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = vlr.mmc.CallFunction<string>(FunctionGenericTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = (string)vlr.mmc.CallFunction(FunctionGenericTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = vlr.mmc.CallFunction<string>(FunctionGenericTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);
            ret = null;
            ret = (string)vlr.mmc.CallFunction(FunctionGenericTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            string[] strings = { "Red", "Blue", "Green" };
            var list1 = strings.ToList();

            fctx = FunctionContext.NewFunctionContext().AddParam("0", list1);
            var list2 = vlr.mmc.CallFunction<List<string>>(FunctionGenericTaskAsyncList.NAME, fctx);
            Assert.IsTrue((list1.Count == list2.Count) && !list1.Except(list2).Any());

            fctx = FunctionContext.NewFunctionContext().AddParam("0", list1);
            list2 = vlr.mmc.CallFunction<List<string>>(FunctionTypedTaskList.NAME, fctx);
            Assert.IsTrue((list1.Count == list2.Count) && !list1.Except(list2).Any());

            fctx = FunctionContext.NewFunctionContext().AddParam("0", list1);
            list2 = vlr.mmc.CallFunction<List<string>>(FunctionRegularList.NAME, fctx);
            Assert.IsTrue((list1.Count == list2.Count) && !list1.Except(list2).Any());

        }

        [Test]
        public void TestCallFunctionMultiThread()
        {
            List<Task<bool>> tasks = new List<Task<bool>>();
            int tests = 10;
            for (int i = 0; i < tests; i++)
            {
                tasks.Add(RunAsyncForFunctionRegular(i, FunctionTypedTaskAsyncMultiThread.NAME));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var z in tasks)
            {
                Assert.IsTrue(z.Result == true);

            }
            tasks.Clear();
            for (int i = 0; i < tests; i++)
            {
                tasks.Add(RunAsyncForFunctionAsync(i, FunctionTypedTaskAsyncMultiThread.NAME));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var z in tasks)
            {
                Assert.IsTrue(z.Result == true);

            }
            tasks.Clear();
            for (int i = 0; i < tests; i++)
            {
                tasks.Add(RunAsyncForFunctionRegular(i, FunctionRegularMultiThread.NAME));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var z in tasks)
            {
                Assert.IsTrue(z.Result == true);

            }
            tasks.Clear();
            for (int i = 0; i < tests; i++)
            {
                tasks.Add(RunAsyncForFunctionAsync(i, FunctionRegularMultiThread.NAME));
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var z in tasks)
            {
                Assert.IsTrue(z.Result == true);

            }
            tasks.Clear();
        }

        [Test]
        public void TestCallListenerMultiThread()
        {
            List<Task<bool>> tasks = new List<Task<bool>>();
            int tests = 10;

            for (int i = 0; i < tests; i++)
            {
                var random = new Random().Next(1, 3);
                if (i % 10 == 0) random = 3;
                tasks.Add(RunAsyncForListenerRegular(i, random.ToString()));


            }
            Task.WaitAll(tasks.ToArray());
            foreach (var z in tasks)
            {
                Assert.IsTrue(z.Result == true);

            }


            tasks.Clear();


            for (int i = 0; i < tests; i++)
            {
                var random = new Random().Next(1, 3);
                if (i % 10 == 0) random = 3;
                tasks.Add(RunAsyncForListenernAsync(i, random.ToString()));
            }

            Task.WaitAll(tasks.ToArray());
            foreach (var z in tasks)
            {
                Assert.IsTrue(z.Result == true);

            }



        }



        bool WaitUntilTrue(Func<bool> func, int timeoutInMillis, int timeBetweenChecksMillis)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds < timeoutInMillis)
            {
                if (func())
                    return true;
                Thread.Sleep(timeBetweenChecksMillis);
            }
            return false;
        }

        private Task<bool> RunAsyncForFunctionRegular(int i, string functionName)
        {
            int param = i;

            var t = Task.Run<bool>(() =>
              {
                  try
                  {
                      System.Diagnostics.Debug.WriteLine($"Calling {param}");
                      var reti = vlr.mmc.CallFunction<int>(functionName, FunctionContext.NewFunctionContext().AddParam("test", param));
                      return (reti == param);

                  }
                  catch (Exception e)
                  {
                      return e.Message == ($"Error {param}");
                  }
              });
            return t;
        }
        private Task<bool> RunAsyncForFunctionAsync(int i, string functionName)
        {
            int param = i;

            var t = Task.Run<bool>(async () =>
              {
                  try
                  {
                      System.Diagnostics.Debug.WriteLine($"Calling {param}");
                      var reti = await vlr.mmc.CallFunctionAsync<int>(functionName, FunctionContext.NewFunctionContext().AddParam("test", param));
                      return (reti == param);

                  }
                  catch (Exception e)
                  {
                      return (e.Message == $"Error {param}");
                  }
              });
            return t;
        }

        private Task<bool> RunAsyncForListenerRegular(int i, string typed)
        {
            int param = i;
            string type = typed;
            var t = Task.Run<bool>(() =>
              {
                  bool check = true;
                  FunctionContext fctx1 = null;
                  try
                  {
                      System.Diagnostics.Debug.WriteLine($"Calling {param} Typed TESTMULTI{type}");
                      fctx1 = new FunctionContext($"TESTMULTI{type}");
                      fctx1.AddParam("test", param);
                      var rets = vlr.mmc.DispatchToListeners(fctx1);

                      foreach (var ret in rets.Values)
                      {
                          var myvalue = (int)ret;
                          System.Diagnostics.Debug.WriteLine(param + "*************************" + myvalue);
                          //Assert.IsTrue(myvalue == param);
                          check = check && (myvalue == param);
                      }


                  }
                  catch (Exception e)
                  {
                      //Assert.IsTrue(e.Message.CompareTo($"Error {fctx1.type} {param}") == 0);
                      check = check && (e.Message == $"Error {fctx1.type} {param}");
                  }
                  return check;
              });
            return t;
        }
        private Task<bool> RunAsyncForListenernAsync(int i, string typed)
        {
            int param = i;
            string type = typed;
            var t = Task.Run<bool>(async () =>
              {
                  bool check = true;
                  FunctionContext fctx1 = null;
                  try
                  {
                      System.Diagnostics.Debug.WriteLine($"Calling Async {param} Typed TESTMULTI{type}");
                      fctx1 = FunctionContext.NewFunctionContext().AddType($"TESTMULTI{type}").AddParam("test", param);
                      var rets = await vlr.mmc.DispatchAsyncToListeners(fctx1);

                      foreach (var ret in rets.Values)
                      {
                          var myvalue = (int)ret;
                          System.Diagnostics.Debug.WriteLine("*************************" + myvalue);
                          check = check && (myvalue == param);
                      }

                  }
                  catch (Exception e)
                  {
                      //return Task.FromResult(check);
                      check = check && (e.Message == $"Error {fctx1.type} {param}");
                  }
                  return check;
              });

            return t;
        }

        [Test]
        public void TestCallAsyncFunction()
        {
            string ret = null;

            Task.Run(async () =>
            {
                ret = await vlr.mmc.CallFunctionAsync<string>(FunctionTypedTaskAsync.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                ret = await vlr.mmc.CallFunctionAsync<string>(FunctionTypedTask.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                ret = await vlr.mmc.CallFunctionAsync<string>(FunctionRegular.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                ret = await vlr.mmc.CallFunctionAsync<string>(FunctionGenericTaskAsync.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                ret = await vlr.mmc.CallFunctionAsync<string>(FunctionGenericTask.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            string[] strings = { "Red", "Blue", "Green" };
            var list1 = strings.ToList();

            Task.Run(async () =>
            {

                fctx = FunctionContext.NewFunctionContext().AddParam("0", list1);
                var list2 = await vlr.mmc.CallFunctionAsync<List<string>>(FunctionGenericTaskAsyncList.NAME, fctx);
                Assert.IsTrue((list1.Count == list2.Count) && !list1.Except(list2).Any());
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                fctx = FunctionContext.NewFunctionContext().AddParam("0", list1);
                var list2 = await vlr.mmc.CallFunctionAsync<List<string>>(FunctionRegularList.NAME, fctx);
                Assert.IsTrue((list1.Count == list2.Count) && !list1.Except(list2).Any());
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                fctx = FunctionContext.NewFunctionContext().AddParam("0", list1);
                var list2 = await vlr.mmc.CallFunctionAsync<List<string>>(FunctionTypedTaskList.NAME, fctx);
                Assert.IsTrue((list1.Count == list2.Count) && !list1.Except(list2).Any());
            }).GetAwaiter().GetResult();


            Assert.IsTrue(true); //ran to completation
        }

        [Test]
        public void TestDispatcher()
        {
            string ret;
            Dictionary<string, object> listRet;
            fctx = new FunctionContext("TEST1");
            listRet = vlr.mmc.DispatchToListeners(fctx);
            foreach (var rets in listRet.Values)
            {
                ret = (string)rets;
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }

            fctx = new FunctionContext("TEST2");
            listRet = vlr.mmc.DispatchToListeners(fctx);
            foreach (var rets in listRet.Values)
            {
                ret = (string)rets;
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }

            Assert.IsTrue(true); //ran to completation
        }

        [Test]
        public void TestDispatcherAsync()
        {

            Dictionary<string, object> listRet;
            string ret;
            fctx = new FunctionContext("TEST1");

            Task.Run(async () =>
            {
                listRet = await vlr.mmc.DispatchAsyncToListeners(fctx);
                foreach (var rets in listRet.Values)
                {
                    ret = (string)rets;
                    Assert.IsTrue(ret.CompareTo("Hi") == 0);
                }
            }).GetAwaiter().GetResult();


            fctx = new FunctionContext("TEST2");

            Task.Run(async () =>
            {
                listRet = await vlr.mmc.DispatchAsyncToListeners(fctx);
                foreach (var rets in listRet.Values)
                {
                    ret = (string)rets;
                    Assert.IsTrue(ret.CompareTo("Hi") == 0);
                }
            }).GetAwaiter().GetResult();

            Assert.IsTrue(true); //ran to completation
        }


        [Test]
        public void TestDispatcherSingle()
        {

            string ret;
            fctx = new FunctionContext("TEST1");
            ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerRegular.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            fctx = new FunctionContext("TEST2");
            ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerRegular.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            fctx = new FunctionContext("TEST1");
            ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerGenericTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            fctx = new FunctionContext("TEST2");
            ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerGenericTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);


            fctx = new FunctionContext("TEST1");
            ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerTypedTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            fctx = new FunctionContext("TEST2");
            ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerTypedTaskAsync.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);


            fctx = new FunctionContext("TEST1");
            ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerTypedTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            fctx = new FunctionContext("TEST2");
            ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerTypedTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);


            fctx = new FunctionContext("TEST1");
            ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerGenericTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            fctx = new FunctionContext("TEST2");
            ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerGenericTask.NAME);
            Assert.IsTrue(ret.CompareTo("Hi") == 0);

            Assert.IsTrue(true); //ran to completation
        }

        [Test]
        public void TestDispatcherSingleAsync()
        {

            string ret;

            Task.Run(async () =>
            {
                ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerRegular.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();


            Task.Run(async () =>
            {
                ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerRegular.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerGenericTaskAsync.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();


            Task.Run(async () =>
            {
                ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerGenericTaskAsync.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();


            Task.Run(async () =>
            {
                ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerTypedTaskAsync.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();


            Task.Run(async () =>
            {
                ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerTypedTaskAsync.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();


            Task.Run(async () =>
            {
                ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerTypedTask.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();


            Task.Run(async () =>
            {
                ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerTypedTask.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            Task.Run(async () =>
            {
                ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerGenericTask.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();


            Task.Run(async () =>
            {
                ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerGenericTask.NAME);
                Assert.IsTrue(ret.CompareTo("Hi") == 0);
            }).GetAwaiter().GetResult();

            Assert.IsTrue(true); //ran to completation
        }


        [Test]
        public void TestDispatcherAsyncExcpetion()
        {

            Dictionary<string, object> listRet;
            string ret;
            fctx = new FunctionContext("TEST3");

            Task.Run(async () =>
            {
                try
                {
                    listRet = await vlr.mmc.DispatchAsyncToListeners(fctx);
                    foreach (var rets in listRet.Values)
                    {
                        ret = (string)rets;
                        Assert.IsFalse(ret.CompareTo("Hi") == 0);
                    }
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();
        }

        [Test]
        public void TestDispatcherExcpetion()
        {
            Dictionary<string, object> listRet;
            string ret;
            fctx = new FunctionContext("TEST3");
            try
            {
                listRet = vlr.mmc.DispatchToListeners(fctx);
                foreach (var rets in listRet.Values)
                {
                    ret = (string)rets;
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }

            Assert.IsTrue(true); //ran to completation
        }


        [Test]
        public void TestDispatcherSingleExcpetion()
        {

            string ret;
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerRegular.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerRegular.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerGenericTaskAsync.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerGenericTaskAsync.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerTypedTaskAsync.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerTypedTaskAsync.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }



            try
            {
                fctx = new FunctionContext("TEST3");
                ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerTypedTask.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerTypedTask.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }


            try
            {
                fctx = new FunctionContext("TEST3");
                ret = (string)vlr.mmc.DispatchToSingleListener(fctx, ListenerGenericTask.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }
            try
            {
                fctx = new FunctionContext("TEST3");
                ret = vlr.mmc.DispatchToSingleListener<string>(fctx, ListenerGenericTask.NAME);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
            }


            Assert.IsTrue(true); //ran to completation
        }

        [Test]
        public void TestDispatcherAsyncSingleExcpetion()
        {
            string ret;
            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerRegular.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerRegular.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerGenericTaskAsync.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerGenericTaskAsync.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerTypedTaskAsync.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerTypedTaskAsync.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();


            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerTypedTask.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerTypedTask.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();



            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = (string)await vlr.mmc.DispatchAsyncToSingleListener(fctx, ListenerGenericTask.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext("TEST3");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.DispatchAsyncToSingleListener<string>(fctx, ListenerGenericTask.NAME);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("TEST3 Fails") == 0);
                }
            }).GetAwaiter().GetResult();


            Assert.IsTrue(true); //ran to completation
        }

        [Test]
        public void TestCallAsyncFunctionException()
        {

            string ret;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.CallFunctionAsync<string>(FunctionGenericTask.NAME, fctx);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.CallFunctionAsync<string>(FunctionGenericTaskAsync.NAME, fctx);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.CallFunctionAsync<string>(FunctionRegular.NAME, fctx);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.CallFunctionAsync<string>(FunctionTypedTask.NAME, fctx);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
                }
            }).GetAwaiter().GetResult();

            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            Task.Run(async () =>
            {
                try
                {
                    ret = await vlr.mmc.CallFunctionAsync<string>(FunctionTypedTaskAsync.NAME, fctx);
                    Assert.IsFalse(ret.CompareTo("Hi") == 0);
                }
                catch (Exception e)
                {
                    Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
                }
            }).GetAwaiter().GetResult();



            Assert.IsTrue(true); //ran to completation
        }


        [Test]
        public void TestCallFunctionException()
        {
            string ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = vlr.mmc.CallFunction<string>(FunctionTypedTaskAsync.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = (string)vlr.mmc.CallFunction(FunctionTypedTaskAsync.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = vlr.mmc.CallFunction<string>(FunctionTypedTask.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = (string)vlr.mmc.CallFunction(FunctionTypedTask.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = vlr.mmc.CallFunction<string>(FunctionRegular.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = (string)vlr.mmc.CallFunction(FunctionRegular.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = vlr.mmc.CallFunction<string>(FunctionGenericTaskAsync.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = (string)vlr.mmc.CallFunction(FunctionGenericTaskAsync.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = vlr.mmc.CallFunction<string>(FunctionGenericTask.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            ret = null;
            fctx = new FunctionContext();
            fctx.setParam(0, "Function Fails");
            try
            {
                ret = (string)vlr.mmc.CallFunction(FunctionGenericTask.NAME, fctx);
                Assert.IsFalse(ret.CompareTo("Hi") == 0);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.CompareTo("Fails") == 0);
            }
            Assert.IsTrue(true); //ran to completation
        }


        /*
		[Test]
		public void Fail ()
		{
			Assert.False (true);
		}

		[Test]
		[Ignore ("another time")]
		public void Ignore ()
		{
			Assert.True (false);
		}

		[Test]
		public void Inconclusive ()
		{
			Assert.Inconclusive ("Inconclusive");
		}
		*/
    }
}

