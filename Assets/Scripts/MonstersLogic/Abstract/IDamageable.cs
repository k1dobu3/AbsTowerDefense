public interface IDamageable
{
	bool IsDead();
	void TakeDamage(float damage, bool systemKill);
}
