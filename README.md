﻿# 🐰 Mensageria com RabbitMQ + .NET + Docker

Este projeto é um exemplo simples de comunicação assíncrona usando RabbitMQ com:

- 1 produtor (Producer) que envia mensagens de pedidos
- 3 consumidores (Consumers) que processam os pedidos
- Rodando tudo em contêineres Docker para fácil configuração e escalabilidade.

As mensagens são enviadas para uma fila RabbitMQ através de um Exchange do tipo **Fanout**, onde os consumidores as recebem e processam

---
## Tecnologias Utilizadas

- **Back-end**: .NET 9 (C#)
- **Mensageria**: RabbitMQ
- **Configuração**: Docker Compose
- **Containerização**: Docker

---

## 📂 Estrutura de Pastas

```
MensageriaRabbit/
│
├── docker-compose.yml
├── README.md (Este arquivo)
└── src/
    ├── Producer/
    │   ├── Program.cs
    │   └── Dockerfile
    │   └── Producer.csproj
    │
    ├── Consumer1/
    │   ├── Program.cs
    │   └── Dockerfile
    │   └── Producer.csproj
    │
    ├── Consumer2/
    │   ├── Program.cs
    │   └── Dockerfile
    │   └── Producer.csproj
    │
    └── Consumer3/
        ├── Program.cs
        └── Dockerfile
        └── Producer.csproj

```

---

## Arquitetura
```
Producer  --->  RabbitMQ Exchange (fanout)  --->  Consumer1
                                             --->  Consumer2
                                             --->  Consumer3
```
O produtor envia mensagens para um Exchange do tipo Fanout, que as distribui para todas as filas vinculadas. 
Cada consumidor tem sua própria fila e processa as mensagens de forma independente.


## ⚙️ Instalação e Configuração

### 1. Pré-requisitos

Certifique-se de ter instalado:
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com)

---
### 2. Clonar o Repositório

```bash
git clone https://github.com/teteu434/MensageriaRabbit.git
cd MensageriaRabbit
```
---
### 3. Rodar o Projeto

Na pasta raiz do projeto, execute o comando para subir os contêineres Docker:
```bash
docker compose up --build
```
Esse comando irá construir as imagens e iniciar o RabbitMQ, o produtor e os consumidores.

---
### 4. Conectar ao RabbitMQ
Abra no navegador o painel de administração do RabbitMQ para monitorar as filas e mensagens:
```bash
http://localhost:15672
```
Faça o login com:

- **Username**: guest
- **Password**: guest

Aqui você pode ver as filas, mensagens e o status dos consumidores.

---

### 5. Enviar/Receber mensagens

No terminal, confira se todos os containers estão rodando corretamente.
```bash
docker ps
```
É esperado ver os seguintes containers: Producer, Consumer1, Consumer2, Consumer3 e RabbitMQ.

Rode o comando abaixo para conferir os logs do produtor e consumidores, onde você verá as mensagens sendo enviadas e processadas:
```bash
docker compose logs -f producer consumer1 consumer2 consumer3
```
É esperado ver mensagens dos produtores e consumidores indicando que as conexões foram feitas.

Em outro terminal, conecte-se ao container do produtor para enviar mensagens manualmente:
```bash
docker attach producer
```
Digite mensagens e pressione Enter para enviá-las. Você verá as mensagens sendo processadas pelos consumidores no terminal aonde foram executados os logs.
```
Producer: Digite uma mensagem para enviar: 
          Mensagem teste
Producer: Mensagem enviada!
Consumer1: [Consumer 1] Recebido: Mensagem teste
Consumer2: [Consumer 2] Recebido: Mensagem teste
Consumer3: [Consumer 3] Recebido: Mensagem teste
```
---

### 6. Encerrando o ambiente

Para parar os contêineres, use o comando:
```bash
docker compose down
```

---

## 🛠 Funcionalidades
- Comunicação entre serviços via mensageria
- Padrão Publish/Subscribe
- Uso de RabbitMQ com Exchange do tipo Fanout
- Aplicações .NET rodando em containers

Projeto simples, mas já mostra como sistemas distribuídos trocam mensagens de forma desacoplada!

---

Se precisar de mais informações, entre em contato! 🚀

Email: matheushcosta434@gmail.com