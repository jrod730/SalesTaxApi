# SalesTaxApi
An API that generates a string based receipt for given products. This project was created as exam for a job interview.

## Design Approach
Upon receiving the PDF for this exam I originally thought about using the template design pattern but, eventually landed on the strategy pattern. Essentially favoring composition over inheritance. I did this not only from a best practice stand point but, a more practical standpoint as with e-commerence business many things change fairly quickly and as products within the database grow we may be able to use some tax calculation algorithms interchangeably or even create news one with ease. Not to mention, taxes and laws change fairly quickly as well so, in addition to favoring composition, I also made the sales tax percentage configurable with the appSettings.json. This would effectively allow a tax update without having to redeploy a code change. 

### Data Stores
As per the rules of only needing to run a dotnet restore, I decided to use a Sqlite db in order to interact with data. Upon start up the database is seeded with products listed within the PDF etst input. 

### Solution Composition 

The solution is brokendown into many projects here is a breakdown of the functionality for each:

* SalesTaxApi - Provides API endpoints for clients to interact.
* Engines - The engines project does most of the heavy lifting with logic in order to return the correct response to the client.
* Repositories - Interact with the db
* Strategies - This is were the strategy pattern logic lives
* Domain - This is where all the models, enums and interface contracts live for the solution.
* Contracts - Contain the request and response objects that can be used by clients. This is separate from the domain because these contracts can be published as nuget packages giving clients the ability to interact with our api.
* Helps - Contains some helper methods and classes used throughout this solution.
* Tests - The test project has tests related specifically to the logic used for the test inputs within the PDF (bonus points) :)

## Assumptions
Below is a list of assumptions I made while creating this solutions:

* I assumed there would be a client that would be able to send a request with at least the productId and the quantity of products being purchased.
* The client would not have a need for complete CRUD operations, as a result, no delete or update operations exist within this app. This allowed for more time focusing on the key aspects of this exam.
* Test users understand how to use SwaggerUI - the endpoint descriptions have been filled out appropriately and, I have taken the liberty of creating sample requests from the PDF test input to make testing easier. 

## Using This Project
 
 * Clone repo
 * Open solutioon
 * Set SalesTaxApi as start up project

