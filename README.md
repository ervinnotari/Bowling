# Bowling [![Deploy](https://github.com/ervinnotari/Bowling/actions/workflows/heroku_ci.yml/badge.svg?branch=heroku)](http://bowling-painel-on-blazor.herokuapp.com/) [![Build](https://github.com/ervinnotari/Bowling/actions/workflows/dotnetcore.yml/badge.svg)](https://github.com/ervinnotari/Bowling/actions/workflows/dotnetcore.yml) [![Security](https://github.com/ervinnotari/Bowling/actions/workflows/security-code.yml/badge.svg)](https://github.com/ervinnotari/Bowling/actions/workflows/security-code.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ervinnotari_Bowling&metric=alert_status)](https://sonarcloud.io/dashboard?id=ervinnotari_Bowling) 

O Bowling é um pequeno serviço contabilizador de pontos do jogo de boliche. É desenvolvido em C# com [Blazor](https://en.wikipedia.org/wiki/Blazor) usando conceitos de [DDD](https://en.wikipedia.org/wiki/Domain-driven_design) e [SOLID](https://en.wikipedia.org/wiki/SOLID) e esta ativo no [Heroku](https://www.heroku.com/) para testes desta estrutura. É um software feito para aplicacar os conceitos acima citados e conceitos basicos de [DevOps](https://en.wikipedia.org/wiki/DevOps) usando alguns artefatos do [Github Actions](https://docs.github.com/pt/actions).

Para testar a comunicação com o sistema, o painel que está disponivel no [link](http://bowling-painel-on-blazor.herokuapp.com/) está escutando um serviço de mensagens MQTT do [HiveMQ](https://www.hivemq.com/mqtt-protocol/), para utilizalo siga os passos a baixo:

1. Abra o [painel do jogo](http://bowling-painel-on-blazor.herokuapp.com/)
2. Em outra aba, abra o [websocket client](http://www.hivemq.com/demos/websocket-client/)
3. Clique diretamente em _Connect_ 
4. Altere o campo _Topic_ para `bowling/play`
5. No campo _Message_ preencha com o objeto `json` (altere os dados conforme desejar):
   ```application/json 
   {
    "name": "string", 
    "alley": "string",
    "pins": 3,
    "date": "2021-05-05T22:35:24.935Z"
   }
   ```
6. Clique em _Publish_
7. Volte ao painel verique a pagina correspondente a `alley` informada.
