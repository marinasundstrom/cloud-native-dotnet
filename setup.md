# Setup

## .env

Service URLs and connection strings are defined as environment variables in the ``.env`` file.

## SSL

We will extract and use the ASP.NET Core Dev certificate.

Generate ``aspnetapp.pfx``. Remember the password!

```
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password
dotnet dev-certs https --trust
```

```
cd ${HOME}/.aspnet/https
```

Extract private key

```
openssl pkcs12 -in aspnetapp.pfx -nocerts -out localhost.key
```


Extract certificate

```
openssl pkcs12 -in aspnetapp.pfx -clcerts -nokeys -out localhost.crt
```

Remove passphrase from key

```
cp localhost.key localhost.key.bak
openssl rsa -in localhost.key.bak -out localhost.key
```

### Finally

Copy the ``localhost.key`` and ``localhost.crt`` to the root of the solution.