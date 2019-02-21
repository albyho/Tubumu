using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// ChineseMobileAttribute
    /// </summary>
    public class ChineseMobileAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        // @"^(133|153|180|181|189|130|131|132|155|156|185|186|176|134|135|136|137|138|139|150|151|152|158|159|182|183|184|157|187|188|157|187|188|147|178)\d{8}$"
        public ChineseMobileAttribute() :
            base(@"^1\d{10}$")
        { }
    }
}

/*
 * 
电信
中国电信手机号码开头数字
2G/3G号段（CDMA2000网络）133、153、180、181、189
4G号段 177
联通
中国联通手机号码开头数字
2G号段（GSM网络）130、131、132、155、156
3G上网卡145
3G号段（WCDMA网络）185、186
4G号段 176
移动
中国移动手机号码开头数字
2G号段（GSM网络）有134x（0-8）、135、136、137、138、139、150、151、152、158、159、182、183、184。
3G号段（TD-SCDMA网络）有157、187、188
3G上网卡 147
4G号段 178
补充
14号段以前为上网卡专属号段，如中国联通的是145，中国移动的是147等等。
170号段为虚拟运营商专属号段，170号段的 11 位手机号前四位来区分基础运营商，其中 “1700” 为中国电信的转售号码标识，“1705” 为中国移动，“1709” 为中国联通。
卫星通信 1349
 * 
 */
