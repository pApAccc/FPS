/// <summary>
/// 
/// </summary>
namespace FPS.Weapon
{
    public class Pistol : Gun
    {
        protected override void ControlShoot()
        {
            if (canShoot)
            {
                animator.SetTrigger("Fire");
            }
        }
    }
}


