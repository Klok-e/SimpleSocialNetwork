#!/bin/bash

rm -r src/backend_api_client/
openapi-generator generate \
  -i client_api_gen/swagger_api.json \
  -g typescript-angular \
  -o src/backend_api_client/ \
  -p taggedUnions=true,stringEnums=true
