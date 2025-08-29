# TibaRepoSearch

## Background

This is a solution for TIBA candidacy assignment. 

The goal of this project is to create a web application that allows users to search for and favor GitHub repositories.

## Design Issues
1. The assignment specifies a 202 status for the POST /api/favorites endpoint. This is ambiguous because a 202 status typically indicates that a request has been accepted for processing but not yet completed. However, in this case, the operation of adding a favorite repository is expected to be completed immediately. Therefore, a more appropriate status code would be 201 (Created) to indicate that the resource has been successfully created.
2. The assignment suggested requests are missing a user ID, hinting identification of the user by the Authorization header within per-user operations. However, the Authorization header is typically used for authentication purposes rather than user identification. A more suitable approach would be to include a user ID in the request body or as a query parameter to clearly identify the user performing the operation.
3. Favorited repositories are suggested and described as joined tables of the repository and its metadata. However, since the metadata computation causes an eventual update for the favorite repository DTO, it is better to persist it as a document with optional analysis property. While the repo-metadata separation is reasonable in terms of relational database, there is actually no advantage is using a relational database based the current requirements. This is neither correct not agile. Evetually, the client retreives a repository document with optional metadata (analysis). Joining of this data online would be a waste of resources, as that full document is already captured on the offline job.

## Design Decisions
1. Command pattern for use cases, for easier business logic invocation
2. Contract project/package for interfaces and models
3. Use cases are implemented in a single BL package, but are separated in a way that enables package easy package splitting.
4. Configuration is hardcoded in the setup, but with configuration providers in mind by passing IConfiguration to the setup methods.
5. Authentication is mocked, with a middleware that sets a fixed user ID for demonstration purposes, instead of implementing a JWT solution.
6. The API project simplifies the system components structure, however involves both API tier and service tier concerns. Rate-limiting (although not implemented) and authorization are none of the service's concerns. Instead, a reverse proxy or an API gateway pattern should have been applied for such tasks. These tools often come with more infrastructure features such as common cyber attacks preventions (such as DDOS) for enhanced security, without much coding efforts.
7. The repository search result are lazy-loaded for a preconfigured period of time for efficient paging
