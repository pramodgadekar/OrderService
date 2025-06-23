# 🛒 OrderService & NotificationService (.NET 8 Microservices)

## 📖 Overview

This project consists of two microservices built with ASP.NET Core (.NET 8):

- **OrderService**: Accepts and processes customer orders
- **NotificationService**: Receives HTTP notifications when a new order is placed

### Tech Stack

- ✅ .NET 8 Web API
- ✅ MediatR for CQRS pattern
- ✅ Redis for caching order retrieval
- ✅ Polly for retry logic
- ✅ Confluent Kafka for publishing order events

---

## 🧱 Architecture

                     +-----------------+
                     |     Client      |
                     +--------+--------+
                              |
                              v
                     +--------+--------+
                     |  OrderService   |
                     +--------+--------+
                              |
       +----------------------+----------------------+
       |                      |                      |
       v                      v                      v
+-------------------+ +--------------------+ +---------------------+
| NotificationService|      | Redis |                | Kafka |
| POST /notify |        | Cache GET /orders |  | orders.created topic|



---

## ✅ Features

### OrderService

- `POST /orders`
  - Creates a new order
  - Sends HTTP request to NotificationService using Polly retry
  - Publishes order created event to Kafka (`orders.created`)

- `GET /orders/{id}`
  - Retrieves order from Redis if present
  - If not found, returns 404 (simulate DB fallback if needed)

### NotificationService

- `POST /notify`
  - Receives order summary
  - Logs to console (can be extended to email, SMS, etc.)

---

## 🧠 Design Decisions

| Component     | Why                                                                 |
|---------------|----------------------------------------------------------------------|
| MediatR       | Enforces separation of concerns with command/handler structure       |
| Redis         | Caches GET responses (5-minute expiry) to improve performance        |
| Polly         | Adds fault tolerance for external HTTP calls                         |
| Kafka         | Enables asynchronous communication and event-driven design           |

---

## 🧪 Local Setup

### 🔧 Requirements

- .NET 8 SDK
- Redis 
- Kafka- // Kafka Not Integrated
- Kafka topic: `orders.created`

### Redis EndiPoint 
- EndPoints = "redis-15520.c9.us-east-1-4.ec2.redns.redis-cloud.com": 15520
- User = "default"
- Password = "IuFYRolQqKwSuH7TzzOpekD8VCUna6RS"
- https://app.redislabs.com/#/

### 🛠 Assumptions
- Orders are stored in memory (no DB used)
- Redis is used only in GET endpoint
- Kafka are already created on AWS and Configured with 7 Days retention

### 🔌 Run Services
Test through Swagger

```bash
# Terminal 1 - http://localhost:5001
cd NotificationService - Restore and Build
dotnet run  

# Terminal 2 -http://localhost:5151
cd OrderService - Restore and Build
dotnet run

