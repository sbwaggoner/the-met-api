# the-met-api
An simple API to pull art from metmuseum.org, written in C#.

Currently, it pulls an image and basic info from [http://metmuseum.org](http://metmuseum.org). It accompishes this through scraping, since the Met offers no public APIs.

# Code Examples

It is possible to get either a random piece of art:
```
    var api = new MetApi();
    api.GetPaintingPage();
    var painting = api.GetPaintingInfo();
```


Or a specific one by passing in an Id:
```
    var api = new MetApi();
    api.GetPaintingPage();
    var painting = api.GetPaintingInfo(12434);
```

Examples of other functionality can be seen in the tests, including JSON results.

#Big Plans, Big Plans

I hope to eventually pull in list of art based on various criteria to make it more useful. But I mostly built it because I like randomly browsing the Met's collection.

# Demos

A working demo can be seen here:
[brentwaggoner.com/painting](http://brentwaggoner.com/painting/)

JSON results can be seen here:
[brentwaggoner.com/painting/json/api](http://brentwaggoner.com/painting/json/api)
