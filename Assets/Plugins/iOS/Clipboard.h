@ interface Clipboard : NSObject

extern "C"
{
	/*  �ӿڷ���  */
	void _copyTextToClipboard(const char *textList);
}

@end