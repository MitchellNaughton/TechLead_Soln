This README.txt will outline my design and thought process when building this application.

First important bits:
	- When opening these, you want to open the TaxManAPI .sln (hopefully this opens the linked projects with it) 
	  and the TaxManWeb .sln (stand alone)
		- If TaxManAPI .sln doesn't open all 4 projects, you want to then open TaxManDL, TaxManTests and TMDB into it
	- when running, the API may run on a different localHost port to what mine was running on, in which case you can
	  just update the port in the API call in the TaxManWeb soln to match that which the API is running on, on your
	  machine.
	- I'm using a sql db to store the values simply because the sql server object can be spun up nice and simply on
	  MS VS. That and outside of progress openedge DBs, SQL is the other of which I have experience. However, I know
	  again that me using the sql server object on MS VS is effectively me setting up the DB locally. So I'm not sure
	  what this means for opening it up onto a new machine. That being said it looks like i've set it up under an MS
	  standard local DB which might be beneficial - MSSQLLocalDB. If connection strings need updating for whatever reason
	  the connectionString key-value in web.config (soln root level) will need updating.


initial thoughts:
- Web api for the application functionality.
	- this will mean any UI could use the functionality
	- would open avenues for new UIs within our company
	- would also open avenue to just sell api access while clients build own UIs i.e. bigger audience 
	- or can sell API + ui as packaged deal

- Entire API solution will be made up of 4 projects (potential for these to be containers via docker however I won't be doing that here)
	- Web API project (.net framework) entry point, controller and data return being primary functions
	- Test project (to test aspects of web api functionality)
	- Data library (C# class library) which will contain the models (to be used in API proj also) and will contain business logic
	  and data access directories. These understandably will be used to retrieve data, and manipulate data where required
	- SQL Server db (created straight in vs as a local db) consisting of 2 tables for now one for tax values, one for country
	  where tax values has countryid in as a foreign key. Would make application easily configurable for international use.
	  Though I will only be focussing on tax bands given in the spec - income brackets of 5000, 20000 and >20000 with rates of 
	  0%, 20% and 40%.

- The API will initiate the get functionality based on inputs passed in (from UI)

- In Data library, the da files will handle sql query executions to db (conn str retrieved from config file using system.configuration)
  The bo files will have methods to formulate sql queries, and further methods which call these but also calculate data for returning.
	- Dapper NuGet will be downloaded for the interaction with sql. Reason behind this in all honesty is because it's the first
	  thing I used when creating my own .net api in the past and it gave me what I need. I haven't personally done any research in
	  to other options, as I haven't come across a need to yet.

- Web UI solution will be created (.net MVC for ease of using initial skeleton) will be used to create a web page where the user shall
  input the salary and click the calculate button to view salary returns after tax.
	- the ui will take a post request from the user, retrieve the form data (salary input) and then itself make a post request to
	  the API application above, which of course needs to be running in parallel.
	- I will just be calculating gross figures in the API to minimise the work required in an API (although given this is a tax
	  calculator it wouldn't exactly have been a massive thing to have the monthly calculations in. I explain why I would change 
	  this next time further down the document in the lessons learned section).
	- The css and js for this application will just be the bog standard bootstrap stuff which comes with the mvc framework. MVP in 
	  this is the API functionality working as that can be sold without UI. For now basic UI is a good starting point for the
	  add on packages. In real life setting you'd look to home build this to avoid licensing and copyrights being on your application
	  unless you weren't too fussed about that.

- Testing
	- I will create a new class project will be using the xUnit NuGet for my unit testing. MS VS has various unit testing projects you can 
	  create as new, but it just seems much simpler to create a class project, define my directories to split the intended testing
	  routes (DL, API) and use the xunit package in conjunction.
		- Note that I didn't get round to adding any tests in for the API project, only the DL project for now.

- Scalability
	- For this, I would start by ensuring webserver things live on a webserver. By that I mean the CSS, JS, images, fonts
	  and any of that good stuff live on a server dedicated to acting as a webserver. It will likely be built for this purpose
	  and hence be quick at handling it. Furthemore, it allows the ASP servers to focus on the server side code and processing.
	- Code optimisation, although this is just a given anyway. You could look at making use of tools such as performance profilers
	  which allow you to see which parts of your code are taking longest to run. Giving you opportunity to analyse and review to
	  see if anything could be done better in comparison to when it was built.
	- Containerisation, this would ensure that parts of the entire system are of their own accord and have a specific purpose. 
	  It would also provide a massive benefit to scalablilty as it will mean we can write code and applications specific to certain
	  environments and run them on any machine needed. I.e. an apache webserver being cli based would likely be easier to use on
	  linux. Containerisation apps such as docker would allow us to write and spin up those instances on a windows machine and allow
	  it to run.
	- DB caching, this is something that I would normally think to put in place on my front end ui only. This way you would have
	  the cached data in the place that uses it the most and displays it to the user, cutting out the need to connect elsewhere and
	  invoke connections and functionality. However, with the idea that we plan to sell the API alone, it would make sense to put
	  make sure the API is also caching data to stop it needing to connect to a database each time. Redis is the only one that comes
	  to mind in the first instance. While it is a key-value string, you can effectively store "objects" as json, so would be able to
	  store and hence link together our country and tax tables if needed, as well as store the calculation end results to just
	  avoid the calculation happening at all unnecessarily. Docker also has a Redis image I believe, and the API/UI would be able
	  to share the same cache.
	- Just a note on the first point, docker would allow for spinning up these instances nice and easily, i.e. a webserver container
	  where the content files live. It could spin up the redis image for the db caching. Say your system needs an sftp server, 
	  and an smtp server then your dockerfile(s) could likely handle all that.

- Security
	- Server side validation 100%, client side is good as a first line of def and good for UX, however isn't a real defecnce 
	  against attackers. Sanitisation of inputs to prevent XSS and other injections. Making use of simple measures such as 
	  html and URL encloding. Also look into whitelisting input values where possible.
	- https response headers (CSP, HSTS etc). Though these do help and filter out inexperienced attackers, they shouldn't be 
	  relied on alone. Though definitely a good thing to try and implement, but server side validation is key.
	- Authentication. Something which you could easily implement with a web based system and an API. Username and passwords
	  being the simple avenue. Or develop an SSO functionality where users will autheticate with their business or
	  network logins, meaning we don't need to store passwords etc.

- Couple of lessons learnt
	- Next time keep all calculation in the API soln with intention of having a cache for the API as well as UI (tho shared)
	  this will ensure speed and performance for clients paying for API only but who may not create their own cache.
	- More initial research into potential DBs other than SQL, very much went off a "what I know" basis.
	- More input validation for sure, both on the web and API projects. That being said I was looking for an MVP here,
	  and so implemented a custom error page as a "catch all". Not the best UX but will do for now and would allow for
	  functionality release at least.
