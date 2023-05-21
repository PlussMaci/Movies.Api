# Movies.Api

Movies.Api is the api endpoint to manage movie lists for different users
Without any db in the background, so if you restart the application then it is really empty
You can create users with the account endpoint, there is login and create user the same

## Installation

docker file creation 

```bash
docker build -f Dockerfile --force-rm -t moviesapi  --label "com.microsoft.created-by=visual-studio" --label "com.microsoft.visual-studio.project-name=Movies.Api" .
```

## Api Endpoints

### account
Just a dummy endpoint to create a JWT token, api/v1.0/account


```javascript
User with the name admin has admin rights other user only user rights
request example:
{
    "UserName":"Test2",
    "Password":"asd"
}
response: 
{
    "token": "some valid JWT token",
    "expires": "2023-05-21T11:19:18.9647584+02:00"
}
```


### movieLists
base endpoint api/v1.0/movieLists


```javascript
Get: api/v1.0/movieLists : returns all the movielists, no login needed
Get: api/v1.0/movieLists/{someGuid} : returns the movie list with the given id, no login needed
Post: api/v1.0/movieLists : creates a movielist object for the logged in user
Put: api/v1.0/movieLists : updates a movilist in the backend will be checked the loggedin user and the creator are the same
Delete: api/v1.0/movieLists/{someGuid} : deletes a movielist, only admin user can delete lists
```
## UI
Simple one page UI added, no session handling, so user should logged in all the time the page is reloaded.
in the background the api endpoint are called
First time you load the page GetAll endpoint is called, if you want search then you need to leave the searchbox or press enter
There is no right check in the UI, error handling is only an alert
