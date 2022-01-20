using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Configuration;
using CoffeeShop.ViewModels;

namespace CoffeeShop
{
    public class Global : INotifyPropertyChanged
    {
        #region private
        private static Global _instance = null;
        private string _themeColor;
        private BaseViewModel _currentPageViewModel = null; // Attribute - Backup fields
        //Helper for Thread Safety
        private static object m_lock = new object();

        #endregion

        public static Global GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Global();
                    }
                }
            }
            return _instance;
        }

        public string NoImageStringSource = "ÿØÿà\0\u0010JFIF\0\u0001\u0001\u0001\0`\0`\0\0ÿÛ\0C\0\u0003\u0002\u0002\u0003\u0002\u0002\u0003\u0003\u0003\u0003\u0004\u0003\u0003\u0004\u0005\b\u0005\u0005\u0004\u0004\u0005\n\a\a\u0006\b\f\n\f\f\v\n\v\v\r\u000e\u0012\u0010\r\u000e\u0011\u000e\v\v\u0010\u0016\u0010\u0011\u0013\u0014\u0015\u0015\u0015\f\u000f\u0017\u0018\u0016\u0014\u0018\u0012\u0014\u0015\u0014ÿÛ\0C\u0001\u0003\u0004\u0004\u0005\u0004\u0005\t\u0005\u0005\t\u0014\r\v\r\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014\u0014ÿÀ\0\u0011\b\u0001,\u0001ƒ\u0003\u0001\"\0\u0002\u0011\u0001\u0003\u0011\u0001ÿÄ\0\u001f\0\0\u0001\u0005\u0001\u0001\u0001\u0001\u0001\u0001\0\0\0\0\0\0\0\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\vÿÄ\0µ\u0010\0\u0002\u0001\u0003\u0003\u0002\u0004\u0003\u0005\u0005\u0004\u0004\0\0\u0001}\u0001\u0002\u0003\0\u0004\u0011\u0005\u0012!1A\u0006\u0013Qa\a\"q\u00142\u0081‘¡\b#B±Á\u0015RÑð$3br‚\t\n\u0016\u0017\u0018\u0019\u001a%&'()*456789:CDEFGHIJSTUVWXYZcdefghijstuvwxyzƒ„…†‡ˆ‰Š’“”•–—˜™š¢£¤¥¦§¨©ª²³´µ¶·¸¹ºÂÃÄÅÆÇÈÉÊÒÓÔÕÖ×ØÙÚáâãäåæçèéêñòóôõö÷øùúÿÄ\0\u001f\u0001\0\u0003\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\0\0\0\0\0\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\vÿÄ\0µ\u0011\0\u0002\u0001\u0002\u0004\u0004\u0003\u0004\a\u0005\u0004\u0004\0\u0001\u0002w\0\u0001\u0002\u0003\u0011\u0004\u0005!1\u0006\u0012AQ\aaq\u0013\"2\u0081\b\u0014B‘¡±Á\t#3Rð\u0015brÑ\n\u0016$4á%ñ\u0017\u0018\u0019\u001a&'()*56789:CDEFGHIJSTUVWXYZcdefghijstuvwxyz‚ƒ„…†‡ˆ‰Š’“”•–—˜™š¢£¤¥¦§¨©ª²³´µ¶·¸¹ºÂÃÄÅÆÇÈÉÊÒÓÔÕÖ×ØÙÚâãäåæçèéêòóôõö÷øùúÿÚ\0\f\u0003\u0001\0\u0002\u0011\u0003\u0011\0?\0ýS¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢±µgñ\u0002ëÚ\u0010Ó!ÓdÑZI†¬÷rÈ·(žSyFÝUJ³y›C\a*\u0002’A$`ìÐ\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001Egxƒ^²ðÎ\u008fs©ê3\b--×s±ëè\0\u001dÉ8\0{×Í>*ý¦¼C©^H4T‹I³\a\bLk,¤z±`Gà\a\u001eõíeÙF+3oØ-\u0017W¢<œvi†Ëííž¯¢ÜúžŠøßþ\u0017ÿ\0Žÿ\0è9ÿ\0’°ÿ\0ñ\u0014\u007fÂÿ\0ñßý\a?òV\u001fþ\"½ÿ\0õC\u001füðûßÿ\0\"xŸëN\u000fù%÷/ó>È¢¾7ÿ\0…ÿ\0ã¿ú\u000e\u007fä¬?üE\u001fð¿üwÿ\0AÏü•‡ÿ\0ˆ£ýPÇÿ\0<>÷ÿ\0È‡úÓƒþI}ËüÏ²(¯\u008dÿ\0á\u007føïþƒŸù+\u000fÿ\0\u0011Gü/ÿ\0\u001dÿ\0Ðsÿ\0%aÿ\0â(ÿ\0T1ÿ\0Ï\u000f½ÿ\0ò!þ´àÿ\0’_rÿ\03ìŠ+ã\u007fø_þ;ÿ\0 çþJÃÿ\0ÄQÿ\0\vÿ\0Ç\u007fô\u001cÿ\0ÉX\u007føŠ?Õ\f\u007fóÃï\u007füˆ\u007f­8?ä—Ü¿Ìû\"Šøßþ\u0017ÿ\0Žÿ\0è9ÿ\0’°ÿ\0ñ\u0014\u007fÂÿ\0ñßý\a?òV\u001fþ\"\u008fõC\u001füðûßÿ\0\"\u001fëN\u000fù%÷/ó>È¢¾Xð¯í5â\u001d6ò5Ö’-ZÌœHV5ŽP=T¨\u0003ð#ŸQ_Kè:õ—‰´‹mOO˜Oip»‘ÇäA\u001dˆ9\u0004zŠð3\u001c£\u0015–5íÖ\u008ffµGµ€Í0Ù\u008dý‹Õt{š\u0014QEx§®\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0\u0014QE\0x?íY«Mo£èZz3,7\u0012Ë+€q’\u0081@ÿ\0ÐÍ|Ù_C~ÖŸó,\u007fÛÏþÒ¯žk÷\u000e\u0019ŠŽWI®·ÿ\0Ò™øÿ\0\u0010I¼Æ¢}-ù ¢Š+ê\u000fœ\n(¢€\n)\tÀ&¾Žøaû9i—\u001a%¶¥âušæêå\u0004«d’\u0018Ò%# 1^Kc\u0019ä\u0001Ò¼¼Ã2Ãå”ÕJï}’Ýž–\a/¯˜Ttè­·od|åE}\u001bñ?ösÓ-ô;\u009dKÃ\v5µÕ²\u0019ZÉä2$ª\u0006HRÜ†ÆqÉ\a¥|â\u000e@4eù–\u001f3¦êP{nžè1Ù}|¾¢§Yo³[1h¢Šõ\u000f4+é_ÙOUšãE×4÷fh­¦ŽT\u0004ç\u0005Ã\u0003ÿ\0 \nùª¾‡ý“>ç‰¿íÛÿ\0j×Ëq4T²º\u008dô·þ”\u008f£áù5˜ÓK­ÿ\0&}\tE\u0014Wâ\aì\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\aÏ_µ§üË\u001föóÿ\0´«çšú\u001bö´ÿ\0™cþÞ\u007fö•|ó_¹pßüŠ©\u007fÛßúS?\u001dÏÿ\0äeWåÿ\0¤ ¢Š+é\u008fž\n+Ïüyñ\tô›ƒ§én¦éOï¦ 0\u008fý\u0090:g×Ò¤ð\u000f\u008fŸZ”ØjN¿më\u0014 \u0005\u0012Žã\u001fÞþu×õZ¾ËÚÛC—ë4ý§³¾§yÓœd×Þ~\u0013ñ\rŸŠ<;a©ØÈ¯oq\u0012°Ûü\rŽTú\u0010r1í_\u0006Vï†<s¯ø5ä:6©=ŠÈrñ®\u001a6>¥X\u0011Ÿ|WÆç¹;Í©AS•¥\u001bÚûk¿ä}fMš,²¤¹ãxË{o¡ö·‹|Cgá\u007f\u000eßêwÒ*[ÛÄÌw\u007f\u001bc…\u001e¤œ\f{×Á\u009dyÆ\rnø£ÇZÿ\0Œž6ÖuIï–3”\u008d°±©õ\n \fûâ°¨È²w”Òš©+ÊV½¶ÓoÌ3œÑfu\"á\u001bF7µ÷×þ\u0018(¢ŠúsçB¾‡ý“>ç‰¿íÛÿ\0j×Ï\u0015ô?ì™÷<Mÿ\0nßûV¾c‰\u007fäWWþÝÿ\0Ò‘ô<?ÿ\0#*_?ý%ŸBQE\u0015øqû\u0010QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\0QE\u0014\u0001ó×íiÿ\02Çý¼ÿ\0í*ùæ¾†ý­?æXÿ\0·Ÿý¥_<×î\\7ÿ\0\"ª_ö÷þ”ÏÇsÿ\0ù\u0019Uù\u007fé(+Šø\u0081ãq Âllœ\u001dFEå‡ü±SßýãÛó«Þ6ñŒ~\u0017²Û\u001eÙ5\t‡î£<…\u001fßoaúšñIç’êy&™ÚYdbÎìrI=Í}æ\u000f\ví\u001f´žß™ñxÌW³^Î\u001bþC\t,Ä’I'$ž¦\u009d\u001c\u008f\f‰$lÑÈ„2²œ\u0010GB)´WÑ\u001e\u0001íÞ\añrx›OÛ)U¿„\u00012tÝèãØþ†ºzù×IÕ®4=B\u001bÛWÛ,g¡èÃºŸc^«\u007fñCM·Ðá»ƒ÷×“/ËkžQ‡]ç°\aó¯œÄàå\u0019¯d®Ÿà}\u0006\u001f\u0017\u0019C÷\u008fToø‡Ä–~\u001b²7\u0017oËq\u001c+÷ä>ƒü{W˜\u008fŠºÏö\u008fž|Ÿ²îÿ\0\u008f]ƒ\u001b}7uÏ¿é\\Î««]kwÏwy)–fü”z\u0001ØUJôh`iÓ\u008f¾®ÙçÖÆN¤½Çd}\u0001áÿ\0\u0011Ùø’Ä\\Z?#‰\"o¿\u0019ô#ú÷­Zù×IÕ®ô;ä»³”Å*ñê\u0018z\u0011ÜWµxGÅ°øªÄÈ±´7\u0011àK\u0019\u0004®}T÷\u001f¨¯+\u0015ƒt}èë\u001fÈôðØ¥[Ý—Ät\u0015ô?ì™÷<Mÿ\0nßûV¾x¯¡\u007fdÙ\u0090\u007fÂK\u0019u\u00120·!r2@ó2qø\u008fÎ¾\u001f‰?äWWåÿ\0¥#ì2\u000fù\u0019Rùÿ\0é,ú\u001aŠ(¯Ã\u008fØ‚Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u0002Š( \u000fž¿kOù–?íçÿ\0iWËÞ*ñE¿…ôÓ<¸’wÊÃ\u000eyvÿ\0\u0001Ü×Ò_¶†³oáý/Ã··$ùiö\u0090\u0015z»\u001f+\n=Í|\a¯k—>\"Ô¤¼ºo™¸HÇÝEì£üó_Ð\\\u001f…uòÊR—Â¯ÿ\0¥3ðþ)Äª9…U\u001f‰Ûÿ\0IE}GQ¸Õ¯¥»º\u0090Ë<‡,Çô\u0003Ð\n¯E\u0015újI+#ó¶ÛwaE\u0014S\u0010RRÑ@\u0005%-ox\u001fC_\u0010xŠ\b%]Öñƒ4£ÕGoÄàTNjœ\\žÈ¨EÎJ+©Òx\u0017áÊßC\u001e£ª¡0·Í\u0015©ãxþó{z\nõ½\u000fÃ÷z¬ÑØi\u001a|·R\u0001òÛÙÄN\aÐt\u001eõ§à\u007f\b]xãÄÖZ5™\u0011´Ç/.8Š1Ë6=‡Aê@¯´ü\u001fàÍ+ÀúDz~•l°D o\u0090ó$­ýçnçüŠü«?â5\u0081j-sMìº%Ýÿ\0ZŸ¦d™\u0003Æ'$ù`·}[ò>Lÿ\0…\u0017ã¯³ùßð\u008fË·\u0019ÛçE»þùÝšåî¬õ\u008f\aêŠ·\u0010^i\u0017ñ\u001dË½Z'\u001eàÿ\0Q_løÏÆZg\u0081t9u=N]‘/Ë\u001cKËÊý‘Gr\u007fN¦¾8ñ÷\u008fµ/ˆzãj\u0017í²5ÊÛÚ¡ÊB™è=Iã'¹ü\u0005y¹&iŽÍ\\\u009djQTûë¿m[¿™èg\u0019v\u000f-QTª7S¶Ÿ~–·‘íß\u0006~>I­]Ã¡x’Eû\\„%µñÂù\u008dÙ\u001f¶ãØ÷èyäûÅ|\a«hÚ\u008f†¯£ƒPµ–ÆèÆ“¬r\f6Ö\u0019Sþzt¯±>\røÉüoà[;ÉßÌ½€›k–ÎIu\u0003æ>åJ“îM|·\u0012e4°ê8Ü*÷%º[_£^Lú<ƒ3«]¼&'âŽÍïèüÑÜQE\u0015ðGÚ\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u0005\u0014Q@\u001e)ûXü\u001f¼ø¹ðÌÅ¤§›­is}²Ö\u001cãÏ\u0018!â\u001eär=Ô\u000eõùŸui=\u008dÌ¶÷\u0010É\u0004ñ1I\"‘J²08 ƒÐ‚:WìÝp¾8ø\u001dàOˆ×FçÄ>\u001a³¾»#\rt¡¢™€è\v¡V8÷5úO\fñrÉ¨¼&&\u000eTïumÕ÷Vvºëº>\u0003ˆ8_ûZªÄáæ£=\u009dö}¶Ùü\u008fÉš+ôãþ\u0018çá\u0017ý\nŸùPºÿ\0ã´\u007fÃ\u001cü\"ÿ\0¡Sÿ\0*\u0017_üv¾çþ\"\u000eWÿ\0>ê}Ñÿ\0ä\u008f\u008dÿ\0Q³\u001fùù\u000f¾_ü‰ù\u008fE~œ\u007fÃ\u001cü\"ÿ\0¡Sÿ\0*\u0017_üv\u008føcŸ„_ô*\u007fåBëÿ\0ŽÑÿ\0\u0011\a+ÿ\0Ÿu>èÿ\0òAþ£f?óò\u001f|¿ù\u0013ó\u001eŠý8ÿ\0†9øEÿ\0B§þT.¿øí\u001fðÇ?\b¿èTÿ\0Ê…×ÿ\0\u001d£þ\"\u000eWÿ\0>ê}Ñÿ\0äƒýFÌ\u007fçä>ù\u007fò'æ=z7Á¸TÜj²ãæ\v\u001a\u000f¡,\u007f¥}åÿ\0\fsð‹þ…Oü¨]\u007fñÚâ~.~Ï^\u0012øoá?ío\thßÙ®'T»o´Í.èÈ!OÎì\u0006\u001b\u0003\u008fïUG\u008drìÂK\vN\u0013R›²m+~\u0012\u007f\u0090¥Â8ì\nxš’‹QÕÙ»ÿ\0é(¹û'ØE&­â+Ò\a\u009d\u00140Â¾ÊÌÄÿ\0è\u0002½ÏÆž4Ó<\v¡Ë©êrì‰~Xâ^^WìŠ;“úu5òÇÁ/‰\u0016¿\u000euíF{õ‘ìn­J•ˆe¼Å;\u0090\u0001ï–\u001fˆ®\u007fÇÞ>Ôþ\"k\u008d¨_¶È×+oj„”\u0081=\a©=Ï\u007fÈWÈãr\u001a¹–m:•t¥e¯}-eó_#êp™Í,\u0006Y\u001atõ©®\u009dµÝ‡\u008f¼}©üC×\u001aþý¶F¹[{T$¤\tžƒÔžç¿ä+Úþ\u0006ü\rþÎû?ˆ¼Eoþ™Ä––2\u000fõ>’8þ÷ þ\u001e½z\u001f\u0003~\u0006ÿ\0g}ŸÄ^\"·ÿ\0LâKK\u0019\aúŸI\u001c\u007f{Ð\u007f\u000f^½=ë¥y¹Þw\nPþÎËô‚Ñµù/ÕõüûòŒ¢u'õìv²z¤ÿ\07ú.‡Î\u001fµ†Ÿ\u0014z\u008f‡/@ýô‘M\v\u001fURŒ?V?\u009di~É÷Nö>\"¶?êãx\u001d~¬\u001c\u001fý\u0004W\u0013ûHxº\u001f\u0011øâ;\vY\u0004–úLf\u0006e9\u0006f9|}0«õ\u0006½7ö^Ðä°ðmö£\"íûuÆ\u0013ý¤AŒÿ\0ßLÃð®¬T]\u000e\u001a…:ß\u0013µ¾rºü\u000el;U¸‚S¥²½þQ³üOg¢Š+ó\u0013ô0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0¢Š(\0ª:Ö\u008fkâ\r&ïN½O2Öê3\u0014‹Ðà÷\a±\u001dAõ\u0015zŠ¨ÉÅ©EÙ¢e\u0015$ã%£>\u001aø\u0081àMCáÿ\0ˆ&ÓïP˜‰-op\aË4yá‡¿¨ìjïÂŸ\u0011øwÂ¾(MGÄ6\u0017\u0017É\u0010\u0006Ý¡\u0001–\u0017þùC÷ˆíÏ\u001dpN1ö\aŠ<%¥xËMk\u001dZÑ.¡ê¤ðÑ·÷•ºƒþMx\u0017Š?e\u009dFÞf“AÔa»ƒ<Cwû¹\0ôÈ\u001b[ëÅ~­‚â\u001c.a‡xlt¹$Õ›Ù?šÛÎú\u001fšâò<N\u0006ºÄ`ãÏ\u0014î–í|ºþg¥\u008fÚ\u001bÀ¿gó?µäÝŒù_d—wÓîãõ¯6ø…ûLË©ZKaáky¬Ö@Uõ\v€\u0004 \u001fùæ£;OûGŸnõÇÿ\0Ã=xçÎÙý\u0090»\u007f¿ö¨qÿ\0¡×]á\u007fÙkQ¸™d×µ\u0018làÏ0Ú\u001f2B=2FÕ>ÿ\05sÃ\aÃØ\t{iUç·K©~\u0011_™¼ñyæ1{\u0018Òä¿[5ø·ù\u001e_ðÿ\0Àº‡Ä\u001f\u0010Åah­åä=ÍËd¬IžXžçÐw?\u009d}±¢èöÞ\u001fÒm4ë4òímc\u0011F½N\ar{“ÔŸSU</á-+ÁºjØé6‰k\u000fV#–‘¿¼Ç©?äVÅ|®wœË5¨”U©Çeú¿ëCé2Œ¦9m6äï9nÿ\0Eýj\u0014QE|Éô!E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\u0001E\u0014P\aÿÙ";
        public string ThemeColor
        {
            get => _themeColor;
            set
            {
                _themeColor = value; // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("ThemeColor");
            }
        }

        #region Panel
        private string _homeColor;
        private string _homeTextColor;

        private string _warehouseColor;
        private string _warehouseTextColor;

        private string _productColor;
        private string _productTextColor;

        private string _receiptColor;
        private string _receiptTextColor;

        private string _statisticColor;
        private string _statisticTextColor;

        private string _settingColor;
        private string _settingTextColor;

        public string HomeColor { get => _homeColor; set { _homeColor = value; OnPropertyChanged("HomeColor"); } }
        public string HomeTextColor { get => _homeTextColor; set { _homeTextColor = value; OnPropertyChanged("HomeTextColor"); } }

        public string WarehouseColor { get => _warehouseColor; set { _warehouseColor = value; OnPropertyChanged("WarehouseColor"); } }
        public string WarehouseTextColor { get => _warehouseTextColor; set { _warehouseTextColor = value; OnPropertyChanged("WarehouseTextColor"); } }

        public string ProductColor { get => _productColor; set { _productColor = value; OnPropertyChanged("ProductColor"); } }
        public string ProductTextColor { get => _productTextColor; set { _productTextColor = value; OnPropertyChanged("ProductTextColor"); } }

        public string ReceiptColor { get => _receiptColor; set { _receiptColor = value; OnPropertyChanged("ReceiptColor"); } }
        public string ReceiptTextColor { get => _receiptTextColor; set { _receiptTextColor = value; OnPropertyChanged("ReceiptTextColor"); } }

        public string StatisticColor { get => _statisticColor; set { _statisticColor = value; OnPropertyChanged("StatisticColor"); } }
        public string StatisticTextColor { get => _statisticTextColor; set { _statisticTextColor = value; OnPropertyChanged("StatisticTextColor"); } }

        public string SettingColor { get => _settingColor; set { _settingColor = value; OnPropertyChanged("SettingColor"); } }
        public string SettingTextColor { get => _settingTextColor; set { _settingTextColor = value; OnPropertyChanged("SettingTextColor"); } }

        public BaseViewModel CurrentPageViewModel { get => _currentPageViewModel; set { _currentPageViewModel = value; OnPropertyChanged("CurrentPageViewModel"); } }

        #endregion

        #region PropertyChange
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        Global()
        {
            ThemeColor = ConfigurationManager.AppSettings["ThemeColor"];
            CurrentPageViewModel =  HomeViewModel.GetInstance();
        }
    }
}
