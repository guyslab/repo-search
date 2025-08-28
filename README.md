# TibaRepoSearch

## Background

This is a solution for TIBA candidacy assignment. 

The goal of this project is to create a web application that allows users to search for and favor GitHub repositories.

## Design Issues
1. The assignment specifies a 202 status for the POST /api/favorites endpoint. This is ambiguous because a 202 status typically indicates that a request has been accepted for processing but not yet completed. However, in this case, the operation of adding a favorite repository is expected to be completed immediately. Therefore, a more appropriate status code would be 201 (Created) to indicate that the resource has been successfully created.
2. The assignment suggested requests are missing a user ID, hinting identification of the user by the Authorization header within per-user operations. However, the Authorization header is typically used for authentication purposes rather than user identification. A more suitable approach would be to include a user ID in the request body or as a query parameter to clearly identify the user performing the operation.

## Design Decisions
1. Command pattern for use cases, for easier business logic invocation
2. Contract project/package for interfaces and models
3. Use cases are implemented in a single BL package, but are separated in a way that enables package easy package splitting.
4. Configuration is hardcoded in the setup, but with configuration providers in mind by passing IConfiguration to the setup methods.
5. Authentication is mocked, with a middleware that sets a fixed user ID for demonstration purposes, instead of implementing a JWT solution.
