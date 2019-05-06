# CallerCore

[Guide]

[Getting Started Guide](component/GettingStarted.md)

[Introduction]

This library is written in C# codebase for Xamarin. CallerCore has been designed to easy use, flexible design, but at the same time we wanted it to be easily extensible.<br />
CallerCore is based on provider design pattern and Inversion of Control Container (IoC)<br />
CallerCore allows the developers to create pluggable components<br />

To start we must register the functions. Every register is simply a contract between a Function and the Business Logic/Data Abstraction Layer. <br />
The implementation of the Function separate from the Function itself<br />

A Function itself contains no business logic; instead it simply forwards this call to the configured Class. 
CallerCore is the responsibility of the function to contain the implementation for that method, calling whatever Business Logic Layer (BLL) or Data Access Layer (DAL) is necessary.<br />
A Function implementation must derive from an abstract base class, which is used to define a contract for a particular feature<br />
The class that inherits from the abstract base class will do some of the work and the abstract base class will do some of the work.<br />
Every type of function listed above has his abastract that share a common base type. We have three type of functions:<br />
<br />
A) `EnterpriseFunction`:<br /> 
 *   The abstract are AbstractEnterpriseAsync (to implement your method async), AbstractEnterpriseRegular (to implement your regular method), AbstractEnterpriseFunction (virtual implementation of your method async e regular)<br />
 *   callable by callFunction or callFunctionAsync.
<br />    
B) `MessageHandler`:<br />
 *   The abstract is AbstractMessageHandler<br />
 *   callable by callMessageHandler or callMessageHandlerAsync (every function are automatically called using the member type of FunctionContext)<br />
<br />
C) `EnterpriseListener`:<br />
 *   The abstract is AbstractListener<br />
 *   The messages are dispatched by its method addContextToQueue<br />
<br />
Features that make use of Function Class are:<br />
 1)type of application (string example DROID1,DROID2,IOS1). Only "default" and corresponding "type" are loaded in your program<br />
 2)name of function<br />
 3)name of class or name of class and dll separate with "," that implement the function or class type <br />
 4)for listeners we will define the LISTENER_TYPE, for messageHandler we will define the TARGET_TYPE<br />
<br />   
Every call to a classFunction has the mainClass (interface IInfoContext) and FunctionContext that permit to pass parameter from a function to another function.<br />
With FunctionContext we can pass parameters string,class T,int,object ...<br />

Every classFunction has a BaseLogger overridable by your implementation<br />


