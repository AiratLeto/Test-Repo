Интеграционные тесты реализованы для демонстрации. Имеется место с TODO, связанное с тяжестью MsSql. Это можно сделать красиво в docker-compose файле, но пока так в лоб). Доступен сваггер http://localhost:8040/swagger/index.html - Swagger

docker build . -t cars.api
docker run -d -p 8040:80 --name cars-api --env "Application:DbConnectionString"="Server=host.docker.internal,1433;Database=carapitest;User Id=SA;Password=myStrongPassword1.;TrustServerCertificate=True;" cars.api

Вёрстка без дизайна или референса получается только так. Функционально всё работает. Использование state manager показалось избыточным для 3 сущностей. Код можно ещё больше оптимизировать, можно больше создать базовых компонентов для дальнейшего использования.
// TODO: передать через переменную окружения VITE_APP_API_URL не получилось за 30 минут гугла, поэтому при использовании другого порта для бэка, в файле .env нужно будет вручную указать и сбилдить.

docker build . -t cars.web
docker run -d -p 8045:80 --name cars-web --env "VITE_APP_API_URL"="http://localhost:8040" cars.web