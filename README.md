

# Pedagio 
```
http://localhost:5373/swagger/index.html
```

## Thunders.TechTest
```
https://localhost:17114/
```

http://localhost:51134/
Username: guest
Password: guest

## API - Swagger
```
http://localhost:5373/swagger/index.html
```
### Relatorios
GET     ```http://localhost:5373/api/Relatorios/total-por-hora```
GET     ```http://localhost:5373/api/Relatorios/top-pracas```
GET     ```http://localhost:5373/api/Relatorios/veiculos-por-praca```

### Utilizacoes
GET     ```http://localhost:5373/api/Utilizacoes```
POST    ```http://localhost:5373/api/Utilizacoes```
GET     ```http://localhost:5373/api/Utilizacoes/{id}```
PUT     ```http://localhost:5373/api/Utilizacoes/{id}```
DELETE  ```http://localhost:5373/api/Utilizacoes/{id}```



## ConnectionStrings

"ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ThundersTechTestDb;User Id=sa;Password=Mypassword1570!;TrustServerCertificate=True;",
    "RabbitMQ": "amqp://guest:guest@localhost:5672"
  }