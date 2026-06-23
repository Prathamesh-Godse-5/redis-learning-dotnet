using StackExchange.Redis;

// Connect to Redis
var connection = ConnectionMultiplexer.Connect("localhost:6379");
var db = connection.GetDatabase();

Console.WriteLine("Connected to Redis.");

// ── Basic Set & Get ──────────────────────────────────────
db.StringSet("name", "Prathamesh");
db.StringSet("city", "Panvel");

string name = db.StringGet("name");
string city = db.StringGet("city");

Console.WriteLine($"Name: {name}");
Console.WriteLine($"City: {city}");

// ── TTL Example - OTP expires in 30 seconds ──────────────
db.StringSet("otp:9876", "4521", TimeSpan.FromSeconds(30));
Console.WriteLine("\nOTP stored with 30-second expiration.");

string otp = db.StringGet("otp:9876");
Console.WriteLine($"OTP Now: {otp}");

Console.WriteLine("Waiting 31 seconds...");
Thread.Sleep(31000);

string otpAfter = db.StringGet("otp:9876");
Console.WriteLine($"OTP After 31s: {(otpAfter.IsNull ? "EXPIRED!" : otpAfter.ToString())}");

// ── Cache Aside Pattern ───────────────────────────────────
string userId = "user:101";

string cachedUser = db.StringGet(userId);

if (cachedUser == null)
{
    Console.WriteLine("\nCache MISS - Fetching from database...");

    // Simulating database fetch
    string userData = "Prathamesh | Panvel | .NET Developer";

    // Store in Redis with 1 minute expiry
    db.StringSet(userId, userData, TimeSpan.FromMinutes(1));

    Console.WriteLine("Data stored in Redis cache.");
    cachedUser = userData;
}
else
{
    Console.WriteLine("\nCache HIT - Data served from Redis!");
}

Console.WriteLine($"User Data: {cachedUser}");