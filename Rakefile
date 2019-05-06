require "rake/clean"

CLEAN.include "component/*.xam"
CLEAN.include "component/xamarin-component"

file "component/xamarin-component/xamarin-component.exe" do
	puts "* Downloading xpkg..."
	sh "rm -r -f component/xamarin-component"
	mkdir "component/xamarin-component"
	sh "curl -L https://components.xamarin.com/submit/xpkg > xpkg.zip"
	sh "unzip -o -q xpkg.zip -d component/xamarin-component"
	sh "rm xpkg.zip"
end

task :default => "component/xamarin-component/xamarin-component.exe" do
	line = <<-END
	mono component/xamarin-component/xamarin-component.exe package
		END
	puts "* Creating CallerCore Component"
	puts line.strip.gsub "\t\t", "\\\n    "
	sh line, :verbose => false
	puts "* Created CallerCore Component"
end
