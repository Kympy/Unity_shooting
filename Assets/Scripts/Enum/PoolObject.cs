
using System.Collections.Generic;

public enum PoolObjectKey
{
	Bullet,
}
public class PoolObject
{
	private static Dictionary<PoolObjectKey, string> dictionary = new Dictionary<PoolObjectKey, string>()
	{
		{ PoolObjectKey.Bullet, "Bullet/PlayerBullet" },
	};
	public static string GetPath(PoolObjectKey key)
	{
		if (dictionary.ContainsKey(key) == false)
		{
			return null;
		}
		return dictionary[key];
	}
}
