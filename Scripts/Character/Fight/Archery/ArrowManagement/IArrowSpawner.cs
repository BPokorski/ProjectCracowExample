using Items.Managers;

namespace Character.Fight.Archery.ArrowManagement
{
    public interface IArrowSpawner
    {
        public ArrowProjectile GetArrow();

        public void ReleaseArrow(ArrowProjectile arrow);
    }
}