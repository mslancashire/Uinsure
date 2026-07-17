# Uinsure

Tech Test for Uinsure

## How To Run

TBC

## Functional Requirements

- [ ] (Must) Sell a Policy.
- [ ] (Must) Retrieve a Policy.
- [ ] (Should) Cancel the Policy.
- [ ] (Should) Renew a Policy.
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
1. [ ] Build out the Unit test project.
1. [ ] Build out the Integration test project.
1. [ ] POST for selling a policy.
1. [ ] GET for retrieving a policy.
1. [ ] PUT for cancelling a policy, using PUT over DELETE, POST, PATCH as the policy will remain but will be changed.
1. [ ] POST for renewing a policy, question here, would we create a new linked policy or just change the dates.
1. [ ] GET for calculating the cost to cancel a policy before the policy has actually been cancelled.

## Sub Tasks

1. [ ] Add in fluent validation.
1. [ ] Add in an object store.
1. [ ] Add in Docker support.

## Questions

- For policy renewal, would we create a new linked policy or just change the dates on the existing policy?
- 