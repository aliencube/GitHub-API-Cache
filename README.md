# GitHub API Cache #

**GitHub API Chache** provides cached result populated from [GitHub][gh] API requests. As a default, [GitHub][gh] only allows 60 API calls per hour for anonymous users. For authenticated users, [GitHub][gh] only allows 5000 API calls per hour. Both are not enough for certain cases. As this stores responses populated from API requests into a web server's cache memory, it minimises the number of API calls so that API limit can be avoided.


## Getting Started ##

**GitHub API Chache** supports three different authentication types &ndash; `Anonymous`, `Basic` and `AuthenticationKey`. Even though it supports the `Basic` authentication type, this is strongly **NOT** recommended to use as your [GitHub][gh] username and password is exposed on the Internet.


### Anonymouse Type ###

You can simply send cURL request on a command prompt like:

```shell
$ curl -i https://hook.aliencube.local/api/ref/{user}/{repo}/{branch}
```

If you are using [jQuery][jquery], this can be possible:

```javascript
$.ajax({
    type: "GET",
    url: "https://hook.aliencube.local/api/ref/{user}/{repo}/{branch}",
    dataType: "json"
})
.done(function(data) {
    // DO STUFF
});
```


### AuthenticationKey Type ###

You can simply send cURL request on a command prompt like:

```shell
$ curl -H "Authorization: token OAUTH-TOKEN" https://hook.aliencube.local/api/ref/{user}/{repo}/{branch}
```

If you are using [jQuery][jquery], this can be possible:

```javascript
$.ajax({
    type: "GET",
    url: "https://hook.aliencube.local/api/ref/{user}/{repo}/{branch}",
    dataType: "json",
    headers: { "Authorization": "token OAUTH-TOKEN" }
})
.done(function(data) {
    // DO STUFF
});
```


## Configurations ##

In order to change some configurations, `Web.config` needs to be touched.

```xml
<applicationSettings>
    <Aliencube.AlienCache.WebApi.Properties.Settings>
        <setting name="TimeSpan" serializeAs="String">
            <value>60</value>
        </setting>
        <setting name="UseAbsoluteUrl" serializeAs="String">
            <value>False</value>
        </setting>
    </Aliencube.AlienCache.WebApi.Properties.Settings>

    <Aliencube.GitHub.Cache.Services.Properties.Settings>
        <setting name="UseProxy" serializeAs="String">
            <value>False</value>
        </setting>
        <setting name="ProxyUrl" serializeAs="String">
            <value />
        </setting>
        <setting name="BypassOnLocal" serializeAs="String">
            <value>True</value>
        </setting>
        <setting name="AuthenticationType" serializeAs="String">
            <value>Anonymous</value>
        </setting>
    </Aliencube.GitHub.Cache.Services.Properties.Settings>
</applicationSettings>
```

* `TimeSpan`: Duration for how long the cache value is alive, in seconds. Default value is `60`.
* `UseAbsoluteUrl`: If it is set to `true`, the cache key will use the fully qualified URL to store cache value. Default value is `false`.
* `UseProxy`: If your application sits behind a firewall, this value must be set to `true`; otherwise the application will not hit [GitHub][gh] APIs. Default value is `false`.
* `ProxyUrl`: Proxy server URL. This is necessary, if `UseProxy` value is `true`. If `UseProxy` valus is `false`, this is ignored.
* `BypassOnLocal`: If it is set to `true` when `UseProxy` value is `true`, all local traffic is bypassed. If `UseProxy` value is `false`, this is ignored.
* `AuthenticationType`: This determines how [GitHub][gh] API is consumed. This value can be `Anonymous`, `Basic` and `AuthenticationKey`. Default value is `Anonymous`. 


## License ##

**GitHub API Chache** is released under [MIT License](http://opensource.org/licenses/MIT).

> The MIT License (MIT)
> 
> Copyright (c) 2014 [aliencube.org](http://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
> furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

[gh]: http://github.com
[jquery]: http://jquery.com
