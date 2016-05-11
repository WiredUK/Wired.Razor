#Wired.Razor#
A library for parsing Razor in an MVC app, and also if needed without relying on an MVC context. Additionally a library for exporting views as PDF files.

##Usage##

1. Add a reference to this library (duh!) by either:

    a. Downloading this codebase and compiling it, or instead I recommend;

    b. Use the [Nuget package](https://www.nuget.org/packages/Wired.Razor) by using the Nuget Package Manager in Visual Studio or running this in the package manager console:

        Install-Package Wired.Razor

2. Create an instance of the parser:

	```c#
    using Wired.Razor;
	
    //This can go anywhere, or preferably be injected
    var parser = new Parser();
	```

3. Now assuming you have a model like this:

	```c#
    public class FoodModel
    {
      public string FoodName { get; set; }
    }
	```

4. And a view like this:

	```html
    @model MyApp.FoodModel
    
    <p>Hello, this is my view and my favourite food is @Model.FoodName</p>
	```

5. You can get your rendered output like this:

	```c#
    //Create our model
    var model = new FoodModel
    {
      FoodName = "Cheese"
    };
    
    //Render the view
    var renderedView = parser.RenderView(viewName, model);
	```

Easy enough right?

##Something a bit more complicated##

Well life is never quite that simple is it? One thing that RazorEngine needs help with is locating any layout files you have. As it has no MVC context, that means there is no View engine to tell it where to look for layouts and such. So we get around that by telling the Wired.Razor exactly what it needs. So if we have a (slightly) more advanced view like this:

```html
@model MyApp.FoodModel
@{
  Layout = "~/Shared/_Layout.cshtml";
}

Hello, this is my view and my favourite food is @Model.FoodName
```

We need change our call to this:

```c#
//Create our model
var model = new FoodModel
{
  FoodName = "Cheese"
};

//Set up the templates
var templates = new List<Template>
{
  new Template
  {
    Name = "~/Views/Shared/_Layout.cshtml",
    Source = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/_Layout.cshtml"))
  }
};

//Render the view
var renderedView = parser.RenderView(viewName, model);
```
	
##What Next?##

Well that's up to you. You can use this approach to build email content for your website. Fed up of writing HTML and doing some weird things with string concatenation? Use Wired.Razor to simplify that process. And as the file it uses are Razor templates, you don't need to recompile your app when the sales director comes to you and says "Hey man, I've been talking to the guys at Initrode and they asked if we could make the emails look purple on Fridays. I told them we can do that as we're a forward thinking dynamic company. So I'm gonna need you to come in on Saturday to make this work mmkay?" Well now you just tweak your templates, tell your boss you worked all Saturday while you were really *home with family* | *partying with friends* | *playing videogames*.

##Bonus Round!##

If you do this:

```c#
parser.Debug = true;
```

Then you can fire up Visual Studio, press F5 and step through your views as they are parsed, just like you would for any other view in your project. Just remember to turn it off for production as it's not optimised when in debug mode. I would actually recommend doing this:

```c#
#if DEBUG
parser.Debug = true;
#endif
```

As this will automatically set debug on in your testing environment, but when you compile it for release mode, it'll be back to it's usual speedy self.

#Didn't you say something about PDFs?#

Of course I did, and here's how you do it. So there's 2 different ways here. They both essentially do the same thing but one of them relies on an MVC `ControllerContext`. This means it works great in your MVC app, but if you want to farm out your PDF generation to a service or a background job using something like the awesome [Hangfire](http://hangfire.io/), then you need to be able to do everything standalone.

The benefit of using the MVC bound version is that you get the nice helpers that MVC provides you with. So things like `@Html.ActionLink(...)`, `@Url.Action(...)` etc.

##Using MVC##

So inside an MVC action, it's simple:

```c#
public ActionResult Index()
{
  var generator = new MvcGenerator(ControllerContext);
  var pdf = generator.GeneratePdf(model, "PdfView");
  return new FileContentResult(pdf, "application/pdf");
}
```

##Standalone##

Only slightly different really: 

```c#
public ActionResult Index()
{
  var generator = new StandaloneGenerator(new Parser());
  var pdf = generator.GeneratePdf(model, Server.MapPath("~/Views/Pdf/ControllerlessPdfWithoutLayout.cshtml"));
  return new FileContentResult(pdf, "application/pdf");
}
```

The difference here is that you are giving the generator an instance of the aformentioned Wired.Razor.Parser class instead of a `ControllerContext` and you specify the full path to the view file. Note that you still need to pass in the templates (`generator.Templates = ...`) as you did above.

##What Else?##

So, images are almost always a huge annoyance in this situation. How does the PDF engine (iTextSharp by the way) know where to get images from? In both the MVC generator and the standalone, there are constructors that take a value for `imageBasePath`. That's a string containing the base path for all images. So say you had in your view something like this:

    <img src="/images/next-weeks-lottery-numbers.jpg" />
    
Well you now need to instantiate the generator like this (in an MVC context):

    var generator = new MvcGenerator(ControllerContext, , Server.MapPath("~"));

This causes your views to be rendered slightly differently, the above would become this:

    <img src="c:\Users\David\Documents\projects\MyApp\images\next-weeks-lottery-numbers.jpg" />
    
##Anything Else?##

I suggest you just give it a try and see how you get on. If you discover a bug then submit an issue and I'll take a look at it. IF you fancy fixing it yourself or adding your own feature, then go right ahead and submit a pull request, I'd absolutely love that!

Good luck!
