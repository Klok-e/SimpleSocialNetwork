#!/bin/bash

dotnet ef migrations add $1 -p DataAccess/ -s SimpleSocialNetworkBack/
