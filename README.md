# Bowling [![Deploy](https://github.com/ervinnotari/Bowling/actions/workflows/heroku_ci.yml/badge.svg?branch=heroku)](http://bowling-painel-on-blazor.herokuapp.com/) [![Build](https://github.com/ervinnotari/Bowling/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/ervinnotari/Bowling/actions/workflows/dotnetcore.yml) [![Security](https://github.com/ervinnotari/Bowling/actions/workflows/security-code.yml/badge.svg)](https://github.com/ervinnotari/Bowling/actions/workflows/security-code.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ervinnotari_Bowling&metric=alert_status)](https://sonarcloud.io/dashboard?id=ervinnotari_Bowling) 

The Bowling is a small service for bowling score counter. It has been developed on C# with [Blazor](https://en.wikipedia.org/wiki/Blazor), using [DDD](https://en.wikipedia.org/wiki/Domain-driven_design) and [SOLID](https://en.wikipedia.org/wiki/SOLID) concepts; it’s active on [Heroku](https://www.heroku.com/) for structure testing. The Bowling is a software made to apply the concept  aforementioned and basic concepts of [DevOps](https://en.wikipedia.org/wiki/DevOps) using artfacts by [Github Actions](https://docs.github.com/pt/actions).

For system communications test, the pannel online on the [link](http://bowling-painel-on-blazor.herokuapp.com/) is listening to a [MQTT](https://pt.wikipedia.org/wiki/MQTT) message service by [HiveMQ](https://www.hivemq.com/mqtt-protocol/). To use it, follow the instructions:

1. Open the [game pannel](http://bowling-painel-on-blazor.herokuapp.com/);
2. In another tab, open  [websocket client](http://www.hivemq.com/demos/websocket-client/);
3. Click on _Connect_;
4. Change the _Topic_ field to `bowling/play`
5. On the _Message_ field fill with `json` object below (change the data as preferred):
   ```application/json 
   {
    "name": "string", 
    "alley": "string",
    "pins": 3,
    "date": "2021-05-05T22:35:24.935Z"
   }
   ```   
6. Click on _Publish_;
7. Go back to the pannel and check the page corresponding to the value described in the `alley` field.
