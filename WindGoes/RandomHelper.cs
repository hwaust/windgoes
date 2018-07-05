using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindGoes
{
	/// <summary>
	/// 提供一些Random类没有的功能。
	/// </summary>
	public class RandomHelper
	{
		/// <summary>
		/// 随机类的对象。
		/// </summary>
		static Random rnd = new Random(); 

		static string surName = "赵钱孙李周吴郑王冯陈褚卫蒋沈韩杨朱秦尤许何吕施张孔曹严华金魏陶姜戚谢邹喻柏水窦章云苏潘葛奚范彭郎鲁韦昌马苗凤花方俞任袁柳酆鲍史唐费廉岑薛雷贺倪汤滕殷罗毕郝邬安常乐于时傅皮卞齐康伍余元卜顾孟平黄和穆萧尹姚邵湛汪祁毛禹狄米贝明臧计伏成戴谈宋茅庞熊纪舒屈项祝董梁杜阮蓝闵席季麻强贾路娄危江童颜郭梅盛林刁钟徐邱骆高夏蔡田樊胡凌霍虞万支柯昝管卢莫经房裘缪干解应宗丁宣贲邓郁单杭洪包诸左石崔吉钮龚程嵇邢滑裴陆荣翁荀羊於惠甄曲家封芮羿储靳汲邴糜松井段富巫乌焦巴弓牧隗山谷车侯宓蓬全郗班仰秋仲伊宫宁仇栾暴甘钭厉戎祖武符刘景詹束龙叶幸司韶郜黎蓟薄印宿白怀蒲台丛鄂索咸籍赖卓蔺屠蒙池乔阴郁胥能苍双闻莘党翟谭贡劳逄姬申扶堵冉宰郦雍却璩桑桂濮牛寿通边扈燕冀郏浦尚农温别庄晏柴瞿阎充慕连茹习宦艾鱼容向古易慎戈廖庚终暨居衡步都耿满弘匡国文寇广禄阙东殴殳沃利蔚越夔隆师巩厍聂晁勾敖融冷訾辛阚那简饶空曾毋沙乜养鞠须丰巢关蒯相查后荆红";
		static string[] surName1 = { "欧阳", "太史", "端木", "上官", "司马", "东方", "独孤", "南宫", "万俟", "闻人", "夏侯", "诸葛", "尉迟", "公羊", "赫连", "澹台", "皇甫", "宗政", "濮阳", "公冶", "太叔", "申屠", "公孙", "慕容", "仲孙", "钟离", "长孙", "宇文", "司徒", "鲜于", "司空", "闾丘", "子车", "亓官", "司寇", "巫马", "公西", "颛孙", "壤驷", "公良", "漆雕", "乐正", "谷梁", "拓跋", "夹谷", "轩辕", "令狐", "段干", "百里", "呼延", "东郭", "南门", "羊舌", "微生", "公户", "公玉", "公仪", "梁丘", "公仲", "公上", "公门", "公山", "公坚", "左丘", "公伯", "西门", "公祖", "第五", "公乘", "贯丘", "公皙", "南荣", "东里", "东宫", "仲长", "子书", "子桑", "即墨", "达奚", "褚师" };
		static string names = "梦琪之桃慕青尔岚初夏沛菡傲珊曼文乐菱惜文香寒新柔语蓉海安夜蓉涵柏水桃醉蓝语琴从彤傲晴语兰又菱碧彤元霜怜梦紫寒妙彤曼易南莲紫翠雨寒易烟如萱若南寻真晓亦向珊慕灵以蕊映易雪柳海云凝天沛珊寒云冰旋宛儿绿真晓霜碧凡夏菡曼香若烟半梦雅绿冰蓝灵槐平安书翠翠风代云梦曼幼翠听寒梦柏醉易访旋亦玉凌萱访卉怀亦笑蓝靖柏夜蕾冰夏梦松书雪乐枫念薇靖雁从寒觅波静曼凡旋以亦念露芷蕾千兰新波代真新蕾雁玉冷卉紫山千琴傲芙盼山怀蝶冰兰山柏翠萱问旋白易问筠如霜半芹丹珍冰彤亦寒之瑶冰露尔珍谷雪乐萱涵菡海莲傲蕾青槐易梦惜雪宛海之柔夏青亦瑶妙菡紫蓝幻柏元风冰枫访蕊芷蕊凡蕾凡柔安蕾天荷含玉书兰雅琴书瑶从安夏槐念芹代曼幻珊谷丝秋翠白晴海露代荷含玉书蕾听白灵雁雪青乐瑶含烟涵双平蝶雅蕊傲之灵薇含蕾从梦从蓉初丹。听兰听蓉语芙夏彤凌瑶忆翠幻灵怜菡紫南依珊妙竹访烟怜蕾映寒友绿冰萍惜霜凌香芷蕾雁卉迎梦元柏代萱紫真千青凌寒紫安寒安怀蕊秋荷涵雁以山凡梅盼曼翠彤谷冬冷安千萍冰烟雅阳友绿南松诗云飞风寄灵书芹幼蓉以蓝笑寒忆寒秋烟芷巧水香映之醉波幻莲夜山芷卉向彤小玉幼南凡梦尔曼念波迎松青寒笑天涵蕾碧菡映秋盼烟忆山以寒寒香小凡代亦梦露映波友蕊寄凡怜蕾雁枫水绿曼荷笑珊寒珊谷南慕儿夏岚友儿小萱紫青妙菱冬寒曼柔语蝶青筠夜安觅海问安晓槐雅山访云翠容寒凡晓绿以菱冬云含玉访枫含卉夜白冷安灵竹醉薇元珊幻波盼夏元瑶迎曼水云访琴谷波笑白妙海紫霜凌旋孤丝怜寒凡松青丝翠安如天凌雪绮菱代云香薇冬灵凌珍沛文紫槐幻柏采文雪旋盼海映梦安雁映容凝阳访风天亦觅风小霜雪萍半雪山柳谷雪靖易白薇梦菡飞绿如波又晴友易香菱冬亦问雁海冬秋灵凝芙念烟白山从灵尔芙迎蓉念寒翠绿翠芙靖儿妙柏千凝小珍妙旋雪枫夏菡绮琴雨双听枫觅荷凡之晓凡雅彤孤风从安绮彤之玉雨珍幻丝代梅青亦元菱海瑶飞槐听露梦岚幻竹谷云忆霜水瑶慕晴秋双雨真觅珍丹雪元枫思天如松妙晴谷秋妙松晓夏宛筠碧琴盼兰小夏安容青曼千儿寻双涵瑶冷梅秋柔思菱醉波醉柳以寒迎夏向雪以丹依凝如柏雁菱凝竹宛白初柔南蕾书萱梦槐南琴绿海沛儿晓瑶凝蝶紫雪念双念真曼寒凡霜飞雪雪兰雅霜从蓉冷雪靖巧翠丝觅翠凡白乐蓉迎波丹烟梦旋书双念桃夜天安筠觅柔初南秋蝶千易安露诗蕊山雁友菱香露晓兰白卉语山冷珍秋翠夏柳如之忆南书易翠桃寄瑶如曼问柳幻桃又菡醉蝶亦绿诗珊听芹新之易巧念云晓灵静枫夏蓉如南幼丝秋白冰安秋白南风醉山初彤凝海紫文凌晴雅琴傲安傲之初蝶代芹诗霜碧灵诗柳夏柳采白慕梅乐安冬菱紫安宛凝雨雪易真安荷静竹代柔丹秋绮梅依白凝荷幼珊忆彤凌青之桃芷荷听荷代玉念珍梦菲夜春千秋白秋谷菱飞松初瑶惜灵梦易新瑶曼梅碧曼友瑶雨兰夜柳芷珍含芙夜云依萱凝雁以莲安南幼晴尔琴飞阳白凡沛萍雪瑶向卉采文乐珍寒荷觅双白桃安卉迎曼盼雁乐松涵山问枫以柳含海翠曼忆梅涵柳海蓝晓曼代珊忆丹静芙绮兰梦安紫丝千雁凝珍香萱梦容冷雁飞柏天真翠琴寄真秋荷代珊初雪雅柏怜容如风南露紫易冰凡海雪语蓉碧玉语风凝梦从雪白枫傲云白梅念露慕凝雅柔盼柳半青从霜怀柔怜晴夜蓉代双以南若菱芷文南晴梦寒初翠灵波问夏惜海亦旋沛芹幼萱白凝初露迎海绮玉凌香寻芹秋柳尔白映真含雁寒松寻雪青烟问蕊灵阳雪巧丹萱凡双孤萍紫菱寻凝傲柏傲儿友容灵枫尔丝曼凝若蕊问丝思枫水卉问梅念寒诗双翠霜夜香寒蕾凡阳冷玉平彤语薇幻珊紫夏凌波芷蝶丹南之双凡波思雁白莲从菡如容采柳沛岚惜儿夜玉水儿半凡语海听莲幻枫念柏冰珍思山凝蕊天玉思萱向梦笑南夏旋之槐元灵以彤采萱巧曼绿兰平蓝问萍绿蓉靖柏。迎蕾碧曼思卉白柏妙菡怜阳雨柏雁菡梦之又莲乐荷寒天凝琴书南映天白梦初瑶平露含巧慕蕊半莲醉卉天菱青雪雅旋巧荷飞丹若灵尔云幻天诗兰青梦海菡灵槐忆秋寒凝凝芙绮山静白尔蓉尔冬映萱白筠冰双访彤绿柏夏云笑翠晓灵含双盼波以云怜翠雁风之卉平松问儿绿柳如蓉曼容天晴丹琴惜天寻琴依瑶涵易忆灵从波依柔问兰山晴怜珊之云飞双傲白沛春雨南梦之笑阳代容友琴雁梅友桃从露语柔傲玉觅夏晓蓝新晴雨莲凝旋绿旋幻香觅双冷亦忆雪友卉幻翠靖柔寻菱丹翠安阳雅寒惜筠尔安雁易飞瑶夏兰沛蓝静丹山芙笑晴新烟笑旋雁兰凌翠秋莲书桃傲松语儿映菡初曼听云初夏雅香语雪初珍白安冰薇诗槐冷玉冰巧之槐夏寒诗筠新梅白曼安波从阳含桃曼卉笑萍晓露寻菡沛白平灵水彤安彤涵易乐巧依风紫南亦丝易蓉紫萍惜萱诗蕾寻绿诗双寻云孤丹谷蓝山灵幻丝友梅从云雁丝盼旋幼旋尔蓝沛山代丝觅松冰香依玉冰之妙梦以冬曼青冷菱雪曼安白千亦凌蝶又夏南烟。靖易沛凝翠梅书文雪卉乐儿傲丝安青初蝶寄灵惜寒雨竹冬莲绮南翠柏平凡亦玉孤兰秋珊新筠半芹夏瑶念文晓丝涵蕾雁凡谷兰灵凡凝云曼云丹彤南霜夜梦从筠雁芙语蝶依波晓旋念之盼芙曼安采珊初柳迎天曼安南珍妙芙语柳含莲晓筠夏山尔容念梦傲南问薇雨灵凝安冰海初珍宛菡冬卉盼晴冷荷寄翠幻梅如凡语梦易梦千柔向露梦玉傲霜依霜灵松诗桃书蝶冰蝶山槐以晴友易梦桃香菱孤云水蓉雅容飞烟雁荷代芙醉易夏烟依秋依波紫萱涵易忆之幻巧水风安寒白亦怜雪听南念蕾梦竹千凡寄琴采波元冬思菱平卉笑柳雪卉谷梦绿蝶飞荷平安孤晴芷荷曼冬尔槐以旋绿蕊初夏依丝怜南千山雨安水风寄柔幼枫凡桃新儿夏波雨琴静槐元槐映阳飞薇小凝映寒傲菡谷蕊笑槐飞兰笑卉迎荷元冬书竹半烟绮波小之觅露夜雪寒梦尔风白梅雨旋芷珊山彤尔柳沛柔灵萱沛凝白容乐蓉映安依云映冬凡雁梦秋醉柳梦凡若云元容怀蕾灵寒天薇白风访波亦凝易绿夜南曼凡亦巧青易。冰真白萱友安诗翠雪珍海之小蕊又琴香彤语梦惜蕊迎彤沛白雁山易蓉雪晴诗珊冰绿半梅笑容沛凝念瑶如冬向真从蓉亦云向雁尔蝶冬易丹亦夏山醉香盼夏孤菱安莲问凝冬萱晓山雁蓉梦蕊山菡南莲飞双凝丝思萱怀梦雨梅冷霜向松迎丝迎梅听双山蝶夜梅醉冬雨筠平文青文半蕾幼菱寻梅含之香之含蕊亦玉靖荷碧萱寒云向南书雁怀薇思菱忆文若山向秋凡白绮烟从蕾天曼又亦依琴曼彤沛槐又槐元绿安珊夏之易槐宛亦白翠丹云问寒易文傲易青旋思真妙之半双若翠初兰怀曼惜萍初之宛丝幻儿千风天蓉雅青寄文代天惜珊向薇冬灵惜芹凌青谷芹雁桃映雁书兰寄风访烟绮晴傲柔寄容以珊紫雪芷容书琴寻桃涵阳怀寒易云采蓝代秋惜梦尔烟谷槐怀莲涵菱水蓝访冬半兰又柔冬卉安双冰岚香薇语芹静珊幻露访天静柏凌丝小翠雁卉访文凌文芷云思柔巧凡慕山依云千柳从凝安梦香旋映天安柏平萱以筠忆曼新竹绮露觅儿碧蓉白竹飞兰曼雁雁露凝冬含灵初阳海秋冰双绿兰盼易思松梦山友灵绿竹灵安凌柏秋柔又蓝尔竹天蓝青枫问芙语海灵珊凝丹小蕾迎夏水之飞珍冰夏亦竹飞莲海白元蝶芷天怀绿尔容元芹若云寒烟听筠采梦凝莲元彤觅山代桃冷之盼秋秋寒慕蕊海亦初晴巧蕊听安芷雪以松梦槐寒梅香岚寄柔映冬孤容晓蕾安萱听枫夜绿雪莲从丹碧蓉绮琴雨文幼荷青柏初蓝忆安盼晴寻冬雪珊梦寒迎南如彤采枫若雁翠阳沛容幻翠山兰芷波雪瑶寄云慕卉冷松涵梅书白乐天雁卉宛秋傲旋新之凡儿夏真静枫乐双白玉问玉寄松丹蝶元瑶冰蝶访曼代灵芷烟白易尔阳怜烟平卉丹寒访梦绿凝冰菱语蕊思烟忆枫映菱凌兰曼岚若枫傲薇凡灵乐蕊秋灵谷槐觅云";

		/// <summary>
		/// 返回一个人的姓名。名字为2-3个字的，不包括复姓
		/// </summary>
		/// <returns></returns>
		public static string GetChineseName()
		{
			string sf = surName[rnd.Next(surName.Length)].ToString();
			string sb = names.Substring(rnd.Next(names.Length / 2) * 2, 2);
			int v = rnd.Next(10);
			if (v == 8)
				sb = sb.Substring(0, 1);
			else if (v == 9)
				sb = sb.Substring(1, 1);

			return sf + sb;
		}

		/// <summary>
		/// 返回一个人的姓名。其中姓为复姓。
		/// </summary>
		/// <returns></returns>
		public static string GetChineseFName()
		{
			string sf = surName1[rnd.Next(surName1.Length)].ToString();
			string sb = names.Substring(rnd.Next(names.Length / 2) * 2, 2);
			int v = rnd.Next(10);
			if (v == 8)
				sb = sb.Substring(0, 1);
			else if (v == 9)
				sb = sb.Substring(1, 1);

			return sf + sb;
		}

		static string vfrontnames = "龙湖俊材金傲柯勤松德鹏煊天佑文博晓啸越彬沙欧顺庆劲松鑫磊荣轩圣杰云舟楷瑞新杰煜祺达强明辉昊然鼎寒金福鑫鹏鸿涛建辉伟诚冠楠炫明智宸明杰智渊哲瀚萧光智轩博超俊驰鹏涛雨泽越泽翰思瑾瑜松德嘉懿智渊天磊雨泽苑博陌昌哲瀚健柏沙欧金傲灵均萧光振豪俊驰志强鑫鹏鹏煊晋鹏睿渊懿轩厉轩风桦菲凡幽朋展鹏炫明卡思旭尧文博俊材松德致轩智轩靖琪晓然彤彤煜城致远俊楠冠霖龙锦";
		static string vbacknames = "庄园盛世星座新城绿洲一号风情小区天地家园华庭公馆花园公寓中苑广场";

		/// <summary>
		/// 返回一个随机的小区的名子。如俊材小区，龙湖庄园等。
		/// </summary>
		/// <returns></returns>
		public static string GetSocietyName()
		{
			int p1 = rnd.Next(vfrontnames.Length / 2);
			int p2 = rnd.Next(vbacknames.Length / 2);
			return vfrontnames.Substring(p1 * 2, 2) + vbacknames.Substring(p2 * 2, 2);
		}

		/// <summary>
		/// 返回一个纯数字组成的字符串。
		/// </summary>
		/// <param name="length">长度。</param>
		/// <returns></returns>
		public static string GetNumberString(int length)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				char c = (char)(48 + rnd.Next(10));
				builder.Append(c);
			}
			return builder.ToString();
		}



		/// <summary>
		/// 随机生成一个数字字符。
		/// </summary>
		/// <returns></returns>
		public static char Number()
		{
			//48-57   数字
			return (char)rnd.Next(48, 58);
		}

		/// <summary>
		/// 随机生成小写字母。
		/// </summary>
		/// <returns></returns>
		public static char LowerChar()
		{
			//97-122  小字母
			return (char)rnd.Next(97, 123);
		}

		/// <summary>
		/// 随机生成大写字母。
		/// </summary>
		/// <returns></returns>
		public static char UpperChar()
		{
			//65-90   大字母
			return (char)rnd.Next(65, 91);
		}

		/// <summary>
		/// 随机生成大小写字母和数据。
		/// </summary>
		/// <returns></returns>
		public static char AnyChar()
		{
			//不分随机生成
			switch (rnd.Next(3))
			{
				case 0:
					return LowerChar();
				case 1:
					return Number();
				case 2:
					return UpperChar();
			}
			return 'a';
		}

		/// <summary>
		/// 随机返回一个指定长度的英文字符串，首字母会大写，后面小写。
		/// </summary>
		/// <param name="length">字符串的长度。</param>
		/// <returns>返回随机长度字符串。</returns>
		public static string AString(int length)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(UpperChar());
			for (int i = 1; i < length; i++)
			{
				sb.Append(LowerChar());
			}
			return sb.ToString();
		}

		/// <summary>
		/// 返回随机颜色。
		/// </summary>
		/// <param name="alpha">表示是否是透明色。</param>
		/// <returns></returns>
		public Color GetColor(bool alpha)
		{
			return alpha ?
				Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256), rnd.Next(256)) :
				Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
		}

		/// <summary>
		/// 随机生成长度为1 - Length的字符串。
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string RandomLengthString(int length)
		{
			StringBuilder sb = new StringBuilder();
			length = rnd.Next(length) + 1;
			for (int i = 0; i < length; i++)
			{
				sb.Append(AnyChar());
			}

			return sb.ToString();
		}

		/// <summary>
		/// 生成固定长度的随机字符串。
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string FixedLengthString(int length)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(UpperChar());
			for (int i = 1; i < length; i++)
			{
				sb.Append(LowerChar());
			}
			return sb.ToString();
		}

		string[] engnames = { "Colma", "Detroit", "Lemon Grove", "Ville De'anjou", "Chicago", "New Haven", "Lieusaint", "Greensboro", "Wenatchee", "Kassel", "Madison Heights", "Cerritos", "Billericay", "Vancouver", "Houston", "New Castle", "Lacey", "Milwaukie", "Anacortes", "Longmont", "Klamath Falls", "Kannapolis", "Bury", "Duvall", "Union City", "Baltimore", "Nevada", "Brisbane", "Tigard", "Stamford", "La Mesa", "Boulogne-Billancourt", "Clackamas", "Gold Coast", "Biloxi", "Palo Alto", "Bellevue", "Great Falls", "Nashua", "Solingen", "Alhambra", "Ontario", "Hillsboro", "Abingdon", "Hollywood", "Jefferson City", "Winston-Salem", "Cergy", "Calgary", "Tooele", "Sandpoint", "Albany", "Milford", "Logansport", "Mentor", "Downey", "Versailles", "Redford", "Carnation", "Randolph", "Bad Soden", "Birmingham", "Bobigny", "Port Macquarie", "Newport News", "San Carlos", "Rockhampton", "Altamonte Springs", "Sparks", "Kansas City", "Ithaca", "Royal Oak", "San Diego", "Branson", "Leipzig", "Darlinghurst", "Sand City", "Sèvres", "Parker", "Plaistow", "Markham", "Verrieres Le Buisson", "Garland", "Sarasota", "Fullerton", "Stafford", "Warrnambool", "Dallas", "Denby", "Ottawa", "Metchosin", "Austell", "Erlangen", "Van Nuys", "Renton", "Neunkirchen", "Sugar Land", "Westland", "Beaverton", "Malabar", "Goulburn", "Hamden", "Saint Matthews", "Runcorn", "Scottsdale", "Maryville", "Philadelphia", "Orange", "Woodbury", "Silverwater", "Ogden", "Columbus", "Laredo", "Melville", "Oxnard", "Münster", "North York", "Carson", "Englewood", "Grevenbroich", "Surprise", "South Bend", "Essen", "New Hartford", "Santa Fe", "Tupelo", "Escondido", "La Grange", "Toronto", "Midland", "Paris", "Destin", "Augusta", "Langford", "Darmstadt", "Cranbourne", "Beverly Hills", "Memphis", "Winnipeg", "Billings", "Brampton", "Johnson Creek", "Austin", "Medford", "Langley", "Mobile", "Drancy", "Hull", "Scarborough", "Bendigo", "Idaho Falls", "Rocky Mount", "Round Rock", "Metz", "Chandler", "Central Valley", "Leeds", "Perth", "Oxon", "Kent", "Minneapolis", "San Gabriel", "Braintree", "Lavender Bay", "East Brisbane", "Peoria", "Visalia", "La Vergne", "Virginia Beach", "Newport Beach", "Indianapolis", "München", "Chantilly", "Fort Worth", "Newport", "Burien", "Clarkston", "Edina", "Ingolstadt", "Whittier", "Wood Dale", "Lake Elsinore", "Springwood", "Daly City", "Gilroy", "Canoga Park", "Orleans", "Sainte-Foy", "Ascheim", "Bonn", "Spring Valley", "West Sussex", "Zeeland", "Woonsocket", "Corpus Christi", "Sunrise", "Troutdale", "Auburn", "Sacramento", "Humble", "Redlands", "Hof", "North Ryde", "Cloverdale", "Lake George", "Cedar City", "Hixson", "Rhodes", "Melton", "Alexandria", "Port Orchard", "Cincinnati", "Peterborough", "Saint Ann", "Cheektowaga", "Daleville", "National City", "Simi Valley", "Woodinville", "Fontana", "Coffs Harbour", "Etobicoke", "Cliffside", "Geelong", "Novi", "Chatou", "North Bend", "Wokingham", "Bellflower", "Issaquah", "Reno", "Richmond", "Salt Lake City", "Rio Rancho", "Greeley", "Ferguson", "Bordeaux", "Moline", "Berks", "Vacaville", "Norridge", "Wrentham", "Dresden", "Cedar Park", "Florence", "Newark", "San Ysidro", "Hayward", "Seattle", "Surrey", "La Jolla", "Waterbury", "North Sydney", "Phoenix", "Nepean", "Citrus Heights", "Outremont", "Eilenburg", "Port Huron", "Trabuco Canyon", "Poing", "Sammamish", "North Randall", "Richmond Hill", "Boise", "Miami", "Basingstoke Hants", "San Francisco", "Riverton", "Berkshire", "Gaffney", "Heath", "Elk Grove", "Vero Beach", "Lake Oswego", "Cambridge", "Mesquite", "Stockton", "La Marque", "Marysville", "Orly", "Shelton", "Bountiful", "Index", "Woolston", "El Segundo", "Euclid", "Kendall", "Spokane", "Michigan City", "Villeneuve-d'Ascq", "North Sioux City", "Norwood", "Tilton", "Fremont", "Kingsport", "Rock Springs", "Newcastle upon Tyne", "North Las Vegas", "Milton Keynes", "Werne", "Seaford", "Bracknell", "St. Leonards", "Warwick", "Yakima", "Colomiers", "Mississauga", "Croix", "Vista", "Imperial Beach", "Everett", "London", "Roubaix", "Monrovia", "Kiel", "Esher-Molesey", "Salzgitter", "Racine", "Somerset", "Long Beach", "Fort Wayne", "Suwanee", "Brossard", "Mesa", "Sillery", "Frankfurt", "Irvine", "San Bruno", "Baldwin Park", "Westport", "College Station", "Bradenton", "Chehalis", "Falls Church", "Millington", "Altadena", "Eureka", "Kenmore", "Lebanon", "Boston", "Saarbrücken", "Sydney", "West Covina", "Offenbach", "Kennewick", "Huntsville", "Gloucestershire", "Melbourne", "Findon", "Lewiston", "Saarlouis", "Park City", "Sunbury", "Saint-Denis", "Clearwater", "Casper", "San Ramon", "Concord", "Sandy", "Endicott", "Aujan Mournede", "Howell", "Snohomish", "Sulzbach Taunus", "East Haven", "Milsons Point", "Oak Bay", "Myrtle Beach", "Duluth", "Saint John", "Hanford", "Warrington", "Hawthorne", "Victoria", "Townsville", "Tuscola", "West Chicago", "Monroe", "Boulogne-sur-Mer", "Cheltenham", "Charlotte", "Tampa", "Chula Vista", "Saint Louis", "Mill Valley", "West Kingston", "Berkeley", "El Cajon", "Springfield", "Saint Ouen", "Chalk Riber", "Smithfield", "Federal Way", "Waterloo", "Edmonds", "York", "San Jose", "Alpine", "Irving", "Farmington", "Barstow", "Shawnee", "Gateshead", "Glendale", "Coronado", "Loveland", "Holland", "Montreal", "High Wycombe", "Orlando", "Odessa", "Kingston", "Lynnwood", "Reading", "City Of Commerce", "Hervey Bay", "Ballard", "Southgate", "Union Gap", "Sequim", "Ellensburg", "Redwood City", "Quebec", "Stoke-on-Trent", "Scotland", "North Miami Beach", "Kirkby", "Salem", "Bremerton", "Saint Germain en Laye", "South Melbourne", "Modesto", "Milwaukee", "Byron", "Bell Gardens", "Gulfport", "New York", "Camarillo", "Santa Monica", "Norwalk", "Wollongong", "Lincoln Acres", "Newton", "Lakewood", "N. Vancouver", "Lille", "Longview", "Washougal", "Baytown", "Elgin", "Kittery", "Gold Bar", "Clay", "Portland", "Morangis", "Milpitas", "Lancaster", "Redmond", "Kanata", "Plano", "Paderborn", "Barrie", "Port Hammond", "Olympia", "Saginaw", "Newport Hills", "Carol Stream", "Westminster", "De Witt", "Valley Stream", "Crossville", "W. York", "Mühlheim", "Cheyenne", "Decatur", "Mosinee", "Oakland", "Sherman Oaks", "Haney", "Grossmont", "Roncq", "Arlington", "Courbevoie", "Les Ulis", "Dorval", "Lakeland", "Puyallup", "Walnut Creek", "Woodland Hills", "Stuttgart", "Campbellsville", "Maidenhead", "Bottrop", "Mcdonough", "Raleigh", "Duesseldorf", "Novato", "Culver City", "Springdale", "Berlin", "Dunkerque", "Pontiac", "Sooke", "Paris La Defense", "Muehlheim", "Torrance", "St. Louis", "Tucson", "Santa Cruz", "Oxford", "Pleasanton", "Oberlin", "Tremblay-en-France", "Gilbert", "Pnot-Rouge", "Bluffton", "Hannover", "Upland", "Frankfurt am Main", "Leesburg", "Mansfield", "Joliet", "Las Vegas", "Braunschweig", "Pantin", "Matraville", "Oregon City", "Savannah", "Newcastle", "Nashville", "Bothell", "Colombes", "Weston", "Burbank", "Los Angeles", "Woodburn", "Merritt Island", "Hooksett", "Edmonton", "Aurora", "Pigeon Forge", "Burlingame", "Marietta", "Saugus", "Augsburg", "Hobart", "Kelso", "Bellingham", "Burnaby", "W. Linn", "Montgomery", "San Mateo", "Kirkland", "Corvallis", "Tacoma", "Branch", "Hamburg", "Roissy en Brie", "Watford", "Missoula", "Suresnes", "Las Cruces", "Carrollton", "Sedro Woolley", "San Antonio", "Santa Ana", "Walla Walla", "Caloundra", "Denver", "Fernley", "Atlanta", "Lane Cove", "Southfield", "Killeen", };

		/// <summary>
		/// 返回一个随机的英文名。
		/// </summary>
		/// <returns></returns>
		public string GetRandomEngName()
		{
			return engnames[rnd.Next(names.Length)];
		}


	}
}
