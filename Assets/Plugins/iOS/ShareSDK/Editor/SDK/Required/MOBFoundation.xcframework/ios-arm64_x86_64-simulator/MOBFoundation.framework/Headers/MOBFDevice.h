//
//  MOBFDeviceUtils.h
//  MOBFoundation
//
//  Created by vimfung on 15-1-19.
//  Copyright (c) 2015年 MOB. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <CoreTelephony/CTTelephonyNetworkInfo.h>
#import <MOBFoundation/MOBSDKDef.h>

extern NSString *kMOBFReachabilityChangedNotification;


/**
 *  设备工具类
 */
@interface MOBFDevice : NSObject


/**
 *  获取单例的 CTTelephonyNetworkInfo
 *
 *  @return CTTelephonyNetworkInfo
 */
+ (CTTelephonyNetworkInfo *)networkInfo;


/**
 *  获取网卡物理地址
 *
 *  @return 网卡物理地址
 */
+ (NSString *)macAddress;

/**
 *  获取设备型号
 *
 *  @return 设备型号
 */
+ (NSString *)deviceModel;

/**
 *  获取设备型号
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 设备型号
 */
+ (NSString *)deviceModel:(BOOL)canCache;

/**
 *  获取当前网络类型
 *
 *  @return 网络类型
 */
+ (MOBFNetworkType)currentNetworkType;

/**
 *  获取当前网络类型
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 网络类型
 */
+ (MOBFNetworkType)currentNetworkType:(BOOL)canCache;

/**
 *  获取当前网络类型字符串
 *
 *  @return 网络类型
 */
+ (NSString *)currentNetworkTypeStr;

/**
 *  获取当前网络类型字符串
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 网络类型
 */
+ (NSString *)currentNetworkTypeStr:(BOOL)canCache;


/**
 *  获取手机运营商代码
 *
 *  @return 手机运营商代码
 */
+ (NSString *)carrier;

/**
 *  获取手机运营商代码
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 手机运营商代码
 */
+ (NSString *)carrier:(BOOL)canCache;

/**
 *  获取手机运营商信息
 *
 *  @return 手机运营商信息
 */
+ (CTCarrier *)curCarrier;

/**
 *  获取手机运营商信息
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 手机运营商信息
 */
+ (CTCarrier *)curCarrier:(BOOL)canCache;

/**
 *  获取手机运营商信息-新生成，不使用缓存
 *
 *  @return 手机运营商信息
 */
+ (CTCarrier *)curCarrierNew;

/**
 *  获取手机运营商名称
 *
 *  @return 运营商名称
 */
+ (NSString *)carrierName;

/**
 *  获取手机运营商名称
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 运营商名称
 */
+ (NSString *)carrierName:(BOOL)canCache;


/**
 *  获取手机运营商名称-指定CTCarrier
 *
 *  @return 运营商名称
 */
+ (NSString *)carrierNameByCarrier:(CTCarrier*)carrier;


/**
 *  获取手机运营商名称-指定CTCarrier
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 运营商名称
 */
+ (NSString *)carrierNameByCarrier:(CTCarrier*)carrier canCache:(BOOL)canCache;

/**
 *  获取手机运营商国家码
 *
 *  @return 运营商国家码
 */
+ (NSString *)mobileCountryCode;

/**
 *  获取手机运营商国家码
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 运营商国家码
 */
+ (NSString *)mobileCountryCode:(BOOL)canCache;

/**
 *  获取手机运营商国家码--指定CTCarrier
 *
 *  @return 运营商国家码
 */
+ (NSString *)mobileCountryCodeByCarrier:(CTCarrier*)carrier;


/**
 *  获取手机运营商国家码--指定CTCarrier
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 运营商国家码
 */
+ (NSString *)mobileCountryCodeByCarrier:(CTCarrier*)carrier canCache:(BOOL)canCache;

/**
 *  获取手机运营商网络编号
 *
 *  @return 运营商网络编号
 */
+ (NSString *)mobileNetworkCode;

/**
 *  获取手机运营商网络编号
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 运营商网络编号
 */
+ (NSString *)mobileNetworkCode:(BOOL)canCache;

/**
 *  获取手机运营商网络编号--指定CTCarrier
 *
 *  @return 运营商网络编号
 */
+ (NSString *)mobileNetworkCodeByCarrier:(CTCarrier*)carrier;


/**
 *  获取手机运营商网络编号--指定CTCarrier
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 运营商网络编号
 */
+ (NSString *)mobileNetworkCodeByCarrier:(CTCarrier*)carrier canCache:(BOOL)canCache;

/**
 *  与当前系统版本比较
 *
 *  @param other 需要对比的版本
 *
 *  @return < 0 低于指定版本； = 0 跟指定版本相同；> 0 高于指定版本
 */
+ (NSInteger)versionCompare:(NSString *)other;

/**
 *  判断是否已经越狱
 *
 *  @return YES 已经越狱，NO 尚未越狱
 */
+ (BOOL)hasJailBroken;

/**
 *  判断是否已经越狱
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return YES 已经越狱，NO 尚未越狱
 */
+ (BOOL)hasJailBroken:(BOOL)canCache;

/**
 *  获取运行进程信息
 *
 *  @return 进程信息列表
 */
+ (NSArray *)runningProcesses;

/**
 *  判断当前设备是否为iPad
 *
 *  @return YES 是，NO 否
 */
+ (BOOL)isPad;

/**
 *  获取设备唯一标识
 *
 *  @return 标识码
 */
+ (NSString *)duid;

/**
 *  获取屏幕真实尺寸
 *
 *  @return 屏幕尺寸
 */
+ (CGSize)nativeScreenSize;


/**
 *  获取屏幕真实尺寸
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 屏幕尺寸
 */
+ (CGSize)nativeScreenSize:(BOOL)canCache;

/**
 *  获取屏幕真实尺寸
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 屏幕尺寸
 */
+ (NSString*)nativeScreenSizeStr:(BOOL)canCache;

/**
 *  获取无线局域网的服务集标识（WIFI名称）
 *
 *  @return 服务集标识
 */
+ (NSString *)ssid;


/**
 *  获取无线局域网的服务集标识（WIFI名称）
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 服务集标识
 */
+ (NSString *)ssid:(BOOL)canCache;

/**
 *  获取基础服务集标识（站点的MAC地址）
 *
 *  @return 基础服务集标识
 */
+ (NSString *)bssid;


/**
 *  获取基础服务集标识（站点的MAC地址）
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 基础服务集标识
 */
+ (NSString *)bssid:(BOOL)canCache;

/**
 *  获取当前语言
 *
 *  @return 语言描述
 */
+ (NSString *)currentLanguage;

/**
 *  获取当前语言
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 语言描述
 */
+ (NSString *)currentLanguage:(BOOL)canCache;

/**
 *  获取设备IP地址
 *
 *  @param ver 版本号
 *  @return IP地址
 */
+ (NSString *)ipAddress:(MOBFIPVersion)ver;

/**
 *  获取设备IP地址
 *
 *  @param ver 版本号
 *  @param type 网络类型
 *  @param canCache 使用冷启动后的缓存
 *  @return IP地址
 */
+ (NSString *)ipAddress:(MOBFIPVersion)ver netType:(MOBFNetType)type useCache:(BOOL)canCache;

/**
 *  获取所有ipv4,ipv6地址
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return IP地址
 */
+ (NSDictionary *)ipAddressAll:(BOOL)canCache;

/**
 *  获取开发商ID
 *
 *  @return 开发商ID
 */
+ (NSString *)idfv;


/**
 *  获取开发商ID
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 开发商ID
 */
+ (NSString *)idfv:(BOOL)canCache;

/**
 *  获取物理内存

 @return 物理内存
 */
+ (double)physicalMemory;

/**
 *  获取物理内存
 *  @param canCache 使用冷启动后的缓存
 @return 物理内存
 */
+ (double)physicalMemory:(BOOL)canCache;

/**
 *  获取存储大小

 @return 存储大小
 */
+ (long long)diskSpace;


/**
 *  获取存储大小
 *  @param canCache 使用冷启动后的缓存
 @return 存储大小
 */
+ (long long)diskSpace:(BOOL)canCache;

/**
 *  获取剩余存储大小

 @return 剩余存储大小
 */
+ (unsigned long long)freeDiskSpace;


/**
 *  获取剩余存储大小
 *
 *  @param canCache 使用冷启动后的缓存
 @return 剩余存储大小
 */
+ (unsigned long long)freeDiskSpace:(BOOL)canCache;

/**
 *  cpu 类型
 *
 *  @return cpu 类型
 */
+ (NSString *)cpuType;

/**
 *  cpu 类型
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return cpu 类型
 */
+ (NSString *)cpuType:(BOOL)canCache;

/**
 *  获取无线局域网的强度
 *
 *  @return 强度 3: 强 ，2：中， 1：弱 ，无
 */
+ (int)wifiLevel;

/**
 *  获取原始当前网络类型
 *
 *  @return 网络类型
 */
+ (NSString *)currentDataNetworkType;


/**
 *  获取原始当前网络类型
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 开发商ID
 */
+ (NSString *)currentDataNetworkType:(BOOL)canCache;

/**
 *  系统版本
 *
 *  @return 系统版本
 */
+ (NSString *)systemVersion;

/**
 *  系统版本
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 系统版本
 */
+ (NSString *)systemVersion:(BOOL)canCache;
/**
 *  系统名称
 *
 *  @return 系统名称
 */
+ (NSString *)systemName;

/**
 *  系统名称
 *
 *  @param canCache 使用冷启动后的缓存
 *  @return 系统名称
 */
+ (NSString *)systemName:(BOOL)canCache;

/**
*  获取设备名称 (如:iPhone 8 Plus)
*
*  @return 设备名称
*/
+ (NSString *)deviceName;

/**
*  获取设备名称 (如:iPhone 8 Plus)
*
*  @param canCache 使用冷启动后的缓存
*  @return 设备名称
*/
+ (NSString *)deviceName:(BOOL)canCache;


/**
*  设备类型
*
*  @return 设备类型
*/
+ (UIUserInterfaceIdiom)userInterfaceIdiom;


/**
*  获取机器名称 (如:x86_64)
*
*  @return 机器名称
*/
+ (NSString *)machineName;

/**
*  获取机器名称 (如:x86_64)
*
*  @param canCache 使用冷启动后的缓存
*  @return 机器名称
*/
+ (NSString *)machineName:(BOOL)canCache;

/**
*  是否连接usb
*
*  @return 是否连接usb
*/
+ (BOOL)isConnUsb:(BOOL)canCache;

/**
*  是否可以充电
*
*  @return 是否可以充电
*/
+ (BOOL)batteryMonitoringEnabled:(BOOL)canCache;


/**
*  是否模拟器
*
*  @return 是否模拟器
*/
+ (BOOL)isSimulator;

/**
*  充电状态
*
*  @param canCache 使用冷启动后的缓存
*  @return 充电状态
*/
+ (UIDeviceBatteryState)batteryState:(BOOL)canCache;


/**
*  屏幕亮度
*
*  @param canCache 使用冷启动后的缓存
*  @return 屏幕亮度
*/
+ (CGFloat)screenBrightness:(BOOL)canCache;

/**
*  时区
*
*  @return 时区
*/
+ (NSString*)timeZone;

/**
*  时区
*
*  @param canCache 使用冷启动后的缓存
*  @return 时区
*/
+ (NSString*)timeZone:(BOOL)canCache;

/**
*  是否设置了代理
*
*  @return 代理
*/
+(BOOL)hasProxy;

/**
*  是否设置了代理
*
*  @param canCache 使用冷启动后的缓存
*  @return 代理
*/
+(BOOL)hasProxy:(BOOL)canCache;

/**
*  cpu个数
*
*  @return 代理
*/
+(unsigned int)cpuCount;

/**
*  字体大小
*
*  @return 默认字体大小
*/
+(CGFloat)fontSize;

/**
*  字体大小
*
*  @param canCache 使用冷启动后的缓存
*  @return 默认字体大小
*/
+(CGFloat)fontSize:(BOOL)canCache;

/**
*  获取启动时间
*
*  @return 获取启动时间
*/
+ (NSNumber *)getBTime;
@end
