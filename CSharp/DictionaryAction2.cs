void Main()
{
	actions["login"]("가나닭").Dump();
	actions["logout"]("마바삵").Dump();
}

private static string Login(string str)
{
	Console.WriteLine("로그인 : " + str);
	return "LOGIN";
}

private static string Logout(string str)
{
	Console.WriteLine("로그아웃 : " + str);
	return "LOGOUT";
}

static Dictionary<string, Func<string, string>> actions = new Dictionary<string, Func<string, string>>()
{
	{ "login", Login },
	{ "logout", Logout }
};
