using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saport2.Business.Entity
{
    public class SaUserModel
    {
        #region About
        /* It includes fields and properties of SampleEntity */
        #endregion

        #region Enums
        // Enums
        #endregion

        #region Notification Messages

        // Email-Phone Empty
        public const string msgPhoneEmailEmpty = "Eposta/Telefon gerekli.";
        // Email-Phone Invalid Format
        public const string msgInvalidPhoneEmailFormat = "Geçersiz Eposta/Telefon veya Şifre.";
        // Password Empty
        public const string msgPasswordEmpty = "Şifre gerekli.";
        // Email/Phone Is Not Valid
        public const string msgInvalidPhoneEmail = "Eposta/Telefon bulunamadı.";
        // Password Is Not Valid
        public const string msgInvalidPassword = "Geçersiz Eposta/Telefon veya Şifre.";
        // After Sending Password SMS
        public const string msgAfterSmsOrEmail = "Şifreniz gönderildi.";
        // Captcha Text Empty or Invalid
        public const string msgCaptchaEmpty= "Gördüğünüz dört karakterli metni yanındaki kutuya yazınız.";
        // User Does Not Exist
        public const string msgUserDoesntExist= "Kullanıcı bulunamadı.";
        // Phone Empty
        public const string msgPhoneEmpty = "Telefon gerekli.";
        // Phone Invalid Format
        public const string msgInvalidPhoneFormat = "Geçersiz Telefon formatı.";
        // Email-Phone Invalid Format
        public const string msgInvalidPhoneOrEmailFormat = "Geçersiz Telefon/Eposta formatı.";
        #endregion


        #region Statics & Constants
        public const string postaGuverciniUserName = "x";
        public const string postaGuverciniPassword = "x";
        public const string usersListName = "Users";
        public const string userDetailsListName = "User Details";
        public const string usersListSiteUrl = "https://saport.x.com/tr-tr/";
        public const string userCamlQueryByGsmNumber = @"<View Scope='RecursiveAll'><Query><QueryOptions><ViewAttributes Scope='Recursive' /></QueryOptions><ViewFields><FieldRef Name='ID'/><FieldRef Name='Name'/><FieldRef Name='Title'/><FieldRef Name='LastName'/><FieldRef Name='Email'/><FieldRef Name='GsmNumber'/><FieldRef Name='Password'/><FieldRef Name='UserActive'/></ViewFields><Where><Eq><FieldRef Name='GsmNumber'/><Value Type='Text'>{0}</Value></Eq></Where></Query></View>";

        public const string userCamlQueryByEmail = @"<View Scope='RecursiveAll'><Query><QueryOptions><ViewAttributes Scope='Recursive' /></QueryOptions><ViewFields><FieldRef Name='ID'/><FieldRef Name='Name'/><FieldRef Name='Title'/><FieldRef Name='LastName'/><FieldRef Name='Email'/><FieldRef Name='GsmNumber'/><FieldRef Name='Password'/><FieldRef Name='UserActive'/></ViewFields>
<Where><Eq><FieldRef Name='Email'/><Value Type='Text'>{0}</Value></Eq></Where></Query></View>";

        public const string userDevicesCamlQuery = @"<View>
<ViewFields><FieldRef Name='UserId'/><FieldRef Name='Title'/></ViewFields>
<Query><Where><Contains><FieldRef Name='UserId'/><Value Type='Number'>{0}</Value></Contains></Where></Query><RowLimit>10</RowLimit></View>";

        public const string setUserNewPassword = @"<Where>
                <Eq>
                   <FieldRef Name='Password' />
                   <Value Type='Text'>{0}</Value>
                </Eq>
              </Where>";
        #endregion

        #region Fields
        // Fields
        #endregion

        #region Properties
        public enum LoginTypeName
        {
            Email,
            Phone,
            None
        }



        public class SaUser
        {
            public LoginTypeName LoginType { get; set; }
            public int? Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string GsmNumber { get; set; }
            public string Password { get; set; }
            public bool UserActive { get; set; }
            public List<SaUserDevice> UserDevices { get; set; }
        }

        public class SaUserDevice
        {
            public int? UserId { get; set; }
            public string DeviceId { get; set; }
        }
        #endregion
    }
}
