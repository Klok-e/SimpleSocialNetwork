#!/bin/bash

rm -r client_api_gen/api/
openapi-generator generate \
  -i client_api_gen/swagger_api.json \
  -g typescript-angular \
  -o src/backend_api_client/
