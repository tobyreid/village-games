# village-games

## Overview

This repository is a playground for me to learn more about ASP.NET Core 2, github best practice and sorting out OAuth2 with IdentityServer4

## Repository Setup

* CODEOWNERS determines who should perform a review in PR / auth merge
* ISSUE_TEMPLATE.md defines the "New Issue" workflow in github
* PULL_REQUEST_TEMPLATE.md defines the "Create Pull Request" workflow in github
* .gitignore defines the ignored files
* .editorconfig ensures that code style is followed

## ASP.NET Solution

### Requirements

Visual Studio 2017 Community 15.6.2 or later with ASP.NET tools

### Getting Started

So long as you have met the requirements, the set both projects as startup and they should both spool up.

There are no external dependencies that need setting up

* Goto the Swagger UI at http://localhost:5001/swagger
* Click "Authorize"
* Click the village-games Scope and proceed with authentication

### Projects

There are two projects in this solution, one deals with authentication, the other with authenticated resources

#### Village Games

The resource server

ASP.NET Core 2 Web API project with standard scaffolding.
API endpoints are protected with IdentityServer - currently configured to look for JWT/Reference tokens.

Swagger UI enhanced by SwashBuckle

#### Village Idiot

The Identity Provider 

ASP.NET Core 2 project with standard scaffolding.

Uses IdentityServer4 to handle authentication and all the well known apis.
