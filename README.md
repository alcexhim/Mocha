Mocha
=====

Build Web sites differently. Mocha (formerly known as [Objectify](https://github.com/alcexhim/Objectify) ) presents an object-oriented view of Web site development designed to appeal to hardcore programmers and traditional Web designers alike.

**Support my projects** by *donating bitcoin* and letting me know what features are most important to you by *opening issues*!
Please send donations for Mocha to `1MBS9KApgZnYmMhwmK7D7CZwHVhwa9uiQN`.

This is the ASP.NET implementation of Mocha. I have plans on eventually merging the PHP implementation and renaming that project permanently, because "Objectify" has too many negative connotations and frankly, "Mocha" sounds cooler... and makes a friendly reference to a similar object-oriented data framework. But for now, I'm focusing developing Mocha as a .NET application so that we can run it pretty much anywhere without too much extra hassle.

The main difference between the ASP.NET version and the PHP version (aside from language) is that the ASP.NET version splits the logic between the Web application component (Mocha.Web) and the Object Management Server component (Mocha.OMS). This way we can drop in different OMS implementations depending on our usage needs without having to heavily modify the front-end application. (You could even use it in a desktop application, if you're into that sort of thing.)

Everything is an Instance
-------------------------

Mocha is built on the principle that "everything is an instance". Instances have Attributes, which are themselves Instances (of the Attribute class), and they are connected between each other via Relationships, which are also Instances (of the Relationship class). And, of course, said "classes" are instances of the Class class; the only thing making them "classes" is the presence of the "has sub Class" relationship.

Hybrid Data Access
------------------

The first time a Mocha instance is accessed, it pulls its data from the database and caches it in memory. Further calls to the same object are based off the in-memory representation. Changes to the object are banged immediately into the database.

History
-------
* All "Properties" (Properties, InstanceProperties, and PropertyValues XQJS sections) have been removed in favor of Attributes and Relationships. EVERYTHING IS AN INSTANCE!

Things to Do
------------
* Finish migrating all logic from the PHP implementation into the .NET implementation. Currently we can log in and run (built-in) reports, but the communication between the Object Management Server (OMS) and the Web application still presents issues.

* Page Components should be generated through `Page Component.has Build Element Method`, rather than relying on hardcoded implementations in multiple locations (PagePage, ExecuteInstancePage, and others actually hard-code multiple DIFFERENT and INCOMPATIBLE implementations of Page Components!)
   
* There should be no real distinction, as there has in the past, between "Objects" and "Instances" (of Objects). EVERYTHING IS AN INSTANCE!
