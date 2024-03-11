# To Start
`dotnet watch run` to run in hot-reloading dev mode.

# Creating credentials
When testing locally, you'll need a token to authenticate. Generate one with:
```
dotnet user-jwts create --scope "limitlist_api" --role "admin"
```
Copy that value into the 'Authorize' window on Swagger (or include as the Authorization Bearer in the request).
