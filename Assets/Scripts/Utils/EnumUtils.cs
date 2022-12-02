using Enums;

namespace Utils
{
    public static class EnumUtils
    {
        public static bool IsEqualColor(BulletColor currentEnum, BulletColor otherEnum) => (currentEnum & otherEnum) == otherEnum;
        public static bool IsEqualColor(BulletSize currentEnum, BulletSize otherEnum) => (currentEnum & otherEnum) == otherEnum;
    }
}
