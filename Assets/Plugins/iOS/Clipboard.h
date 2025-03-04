@ interface Clipboard : NSObject

extern "C"
{
	/*  接口方法  */
	void _copyTextToClipboard(const char *textList);
}

@end