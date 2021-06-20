namespace AYN.Common
{
    public static class AttributeConstraints
    {
        // Category model
        public const int CategoryNameMinLength = 3;
        public const int CategoryNameMaxLength = 20;

        // Ad model
        public const int AdNameMaxLength = 30;
        public const int AdNameMinLength = 2;
        public const int DescriptionMinLength = 10;
        public const int DescriptionMaxLength = 1000;

        // Address model
        public const int AddressNameMaxLength = 50;

        // Post model
        public const int PostTitleMaxLength = 30;
        public const int PostContentMaxLength = 1000;

        // Comment model
        public const int CommentContentMaxLength = 1000;

        // Contact model
        public const int ContactTitleMaxLength = 30;
        public const int ContactContentMaxLength = 1000;

        // Picture model
        public const int PictureExtensionMaxLength = 6;

        // Report model
        public const int ReportContentMinLength = 20;
        public const int ReportContentMaxLength = 1000;

        // Tag model
        public const int TagNameMaxLength = 10;

        // Town model
        public const int TownNameMinLength = 3;
        public const int TownNameMaxLength = 30;

        // ApplicationUser Model
        public const int ApplicationUserUserNameMinLength = 3;
        public const int ApplicationUserUserNameMaxLength = 10;

        public const int ApplicationUserFirstNameMinLength = 3;
        public const int ApplicationUserFirstNameMaxLength = 20;

        public const int ApplicationUserMiddleNameMinLength = 3;
        public const int ApplicationUserMiddleNameMaxLength = 20;

        public const int ApplicationUserLastNameMinLength = 3;
        public const int ApplicationUserLastNameMaxLength = 20;

        public const int ApplicationUserAboutMinLength = 10;
        public const int ApplicationUserAboutMaxLength = 250;

        public const int ApplicationUserSocialContactUrlMaxLength = 25;

        public const int ApplicationUserBlockReasonMinLength = 5;
        public const int ApplicationUserBlockReasonMaxLength = 500;
    }
}
