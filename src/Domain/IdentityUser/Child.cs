
namespace Domain.Models.IdentityUser
{
public class Child : BaseEntity
{
    // ملاحظة: الـ Id من نوع Guid موروث من كلاس Entity الأساسي
    // ويتم إنشاؤه وتمريره عبر الـ Constructor

    // اسم الطفل المستعار - يستخدمه الـ AI لضمان خصوصية الطفل
    public string Nickname { get; private set; }

    // عمر الطفل بالسنوات والشهور (أساسي لتحديد نوع الدعم التربوي)
    public int AgeYears { get; private set; }
    public int AgeMonths { get; private set; }

    // مستوى الاحتياج للدعم (مثلاً: بسيط، متوسط، مكثف)
    public string? SupportLevel { get; private set; }

    // التحديات التي يواجهها الطفل (قائمة كلمات دلالية للـ AI)
    public List<string> Challenges { get; private set; } = [];

    // هل الطفل غير ناطق؟ (تغير نوع استراتيجيات التعامل المقترحة)
    public bool IsNonVerbal { get; private set; }

    // الرابط الفريد (Guid) الخاص بالأم التي ينتمي إليها هذا الطفل
    public int MotherId { get; private set; }
    
    // خاصية التنقل للوصول لبيانات الأم (Navigation Property)
    public virtual AppUser? Mother { get; private set; }

    // كونسلتركتور فارغ مطلوب لعمل الـ Mapping في EF Core
    private Child() : base() { }

    // الكونسلتركتور الأساسي: يتم تمرير Guid جديد للـ base class
    // public Child(string nickname, int ageYears, int ageMonths, Guid motherId) 
    //     : base(Guid.NewGuid()) // هنا بننشئ الـ Id الفريد من نوع Guid ونبعته للـ Entity
    // {
    //     if (string.IsNullOrWhiteSpace(nickname))
    //         throw new ArgumentException("Nickname is required.");

    //     Nickname = nickname;
    //     AgeYears = ageYears;
    //     AgeMonths = ageMonths;
    //     MotherId = motherId;
    // }

    // ميثود لتحديث بيانات الطفل الإضافية
    public void UpdateProfile(string level, List<string> challenges, bool isNonVerbal)
    {
        SupportLevel = level;
        Challenges = challenges;
        IsNonVerbal = isNonVerbal;
    }
}}