# How to run this Example?

1. Start a redis registry.

```bash
docker run --name redis-registry -p 6379:6379 -d redis
```

2. Start provider app `Zoo.Examples.Provider`.
3. Start consumer app `Zoo.Examples.Consumer`.
4. Visit `http://localhost:5001/api/greeting?name=grey`.