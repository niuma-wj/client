#import "PhotoManager.h"
@implementation PhotoManager
- (void)imageSaved : (UIImage*)image didFinishSavingWithError : (NSError*)error
	contextInfo : (void*)contextInfo
{
    NSLog(@"�������");
    if (error != nil)
	{
        NSLog(@"�д���");
    }
}
@end
 
extern "C"
{
	void _SavePhoto(char* readAddr)
	{
		NSString *strReadAddr = [NSString stringWithUTF8String:readAddr];
		UIImage *img = [UIImage imageWithContentsOfFile:strReadAddr];
		PhotoManager *instance = [PhotoManager alloc];
		UIImageWriteToSavedPhotosAlbum(img, instance, @selector(imageSaved:didFinishSavingWithError:contextInfo:), nil);
	}
}