# Uinsure

Tech Test for Uinsure.

## How To Run

Requires .NET 10.

Should run straight out of the box, just for the Tech Test a static object store is used so no need for Docker or SQL.
So build and run the API.

A `Uinsure.Policy.API.http` has been included to show HTTP request examples.
Swagger UI has been enabled which can also be used (`/swagger`).
The `Tests.Uinsure.Integration` project has HTTP examples and tests.

## Functional Requirements

- [x] (Must) Sell a Policy.
- [x] (Must) Retrieve a Policy.
- [ ] (Should) Cancel the Policy.
- [x] (Should) Renew a Policy.
- [ ] (Could) Calculate the cost to cancel a policy before the policy has actually been cancelled.
- [ ] (Could) Not issue a refund if the policy has a claim made against it.
- [ ] (Could) Prevent an auto renewal policy being paid for by cheque.

## Constraints

1. Deliver a REST API written using the policy information defined.
1. Have automated tests.
1. Return informative responses to the consumer.
1. Be shared in a publicly accessible github repo.

## Assumptions

1. I will just commit to the `main` branch, normally `feature/####` branches would be created with a ticket number and then PRs. As `main` would be protected, I would normally create a PR to merge into `main`.
1. Will not be handling any authentication or authorisation.
1. Where user id / customer id is required, it will be passed in the request body or another form. This would normally be a part of a JWT Token Authorisation token, any and all data would only be retrieved based on the user / customer id.


## Tasks

1. [x] Scaffold the project.
1. [x] Update the README.md with the functional requirements, constraints, assumptions and tasks.
1. [x] Build out the Unit test project.
1. [x] Build out the Integration test project.
1. [x] POST for selling a policy.
1. [x] GET for retrieving a policy.
1. [ ] PUT for cancelling a policy, using PUT over DELETE, POST, PATCH as the policy will remain but will be changed.
1. [ ] PUT for renewing a policy, question here, would we create a new linked policy or just change the dates.
1. [ ] GET for calculating the cost to cancel a policy before the policy has actually been cancelled.

## Sub Tasks

1. [x] Add in fluent validation.
1. [x] Add in an object store.
1. [ ] Add in Docker support.

## Questions

- For policy renewal, would we create a new linked policy or just change the dates on the existing policy?
- Upon creating a policy should a Auto Renewal flag be set, currently based on if a payment has been made?


## Package Usage

Some of the below packages can be replaced with custom code to avoid package proliferation.

- `Swashbuckle` for Swagger API UI Explorer.
- `OneOf` for simple result type outputs to avoid Exception Flow application logic. Replacement custom result types.
- `FluentValidation` for validation.
- `SharpGrip.FluentValidation` for adding Auto validation with Action Filters. Can be done using custom logic.
- `xUnit` for unit testing framework, could also be NUnit.
- `AwesomeAssertions` for simple testing, not using FluentAssertions due to being paid for. Replacement would be to just use Assert.That(...).


## Improvements

- The Up To 60 days rule is hard-coded, this could come from configuration or be based on provider.
- Automatically setting the Policy End Date for a year after the Start, this could be configurable.
- How to handle DateOnly with TimeZone.
- How to handle Price and Amount for currencies.
- Use Query, Commands / Service instead of direct controller logic.
- The renewal period logic could be configured instead of hard-coded.
- Move from class based to records domain objects.