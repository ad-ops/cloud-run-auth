# Cloud Run Authentication

## General Questions
How to create a secure and scalable web app?
- How to make sure users on a corporate intranet can view and use easily?
- How to best build and have all functions.

How to use instead of Cloud Functions?
- Which benefits are there? For Python the code is about the same size. Cost is the similar if one-time requests, but if many Cloud Run is better.

When is it appropriate to use Anthos or Kubernetes instead of managed?
- Managed has a cold start which the other can avoid. But at a greater running cost.

## Build & Deploy
### Local
```bash
docker build -t cloud-run-auth .
docker run --rm -p 127.0.0.1:80:80/tcp cloud-run-auth
```

### GCP
```bash
gcloud builds submit --tag eu.gcr.io/helical-bonito-234716/cloud-run-auth .
gcloud run deploy cloud-run-auth --image eu.gcr.io/helical-bonito-234716/cloud-run-auth --platform=managed --region europe-west1
```

## Access to other services
### Local
You can get tokens yourself with `gcloud auth` commands.
`gcloud auth application-default print-access-token`

It is also possible to impersonate a service account in order to try it out.

### GCP runtime environments
When possible use the client libraries and they will take care of many problems. They will automatically get tokens and use them in the library calls. If you want to use a language not supported or a specific funtion that has no library you can try the APIs.

Be careful since the default service account is in general use and might have wide access.

When an application is running inside GCP it can access the its service account and generate tokens from its own metadata.
- https://cloud.google.com/compute/docs/storing-retrieving-metadata
- https://cloud.google.com/compute/docs/access/create-enable-service-accounts-for-instances#applications
- https://cloud.google.com/run/docs/securing/service-identity#fetching_identity_and_access_tokens

With the tokens you can access other apis: https://cloud.google.com/apis

gRPC: https://cloud.google.com/run/docs/triggering/grpc

### Identity token
Some services use identity token, this seems to be mostly created applications such as AppEngine/IAP, Cloud Run, Cloud Function.

### Access token
Access tokens seems to be used for standard GCP services and their APIs.

In order to gain access the scope for the correct service needs to be set and the account granted access in IAM.
You can find all scopes here: https://developers.google.com/identity/protocols/oauth2/scopes
The scope https://www.googleapis.com/auth/cloud-platform has access to all APIs on GCP. You still need to have permission through IAM.

On the API page it states which scopes are needed and in the product which IAM permissions is needed and what roles has them.   

You can look at the access token like this:
`curl -H "Content-Type: application/x-www-form-urlencoded" -d "access_token=$(gcloud auth application-default print-access-token)" https://www.googleapis.com/oauth2/v1/tokeninfo`
