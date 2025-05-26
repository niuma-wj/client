//
//  MobSDK+Privacy.h
//  MOBFoundation
//
//  Created by liyc on 2020/1/21.
//  Copyright © 2020 MOB. All rights reserved.
//

#import <MOBFoundation/MobSDK.h>
#import <UIKit/UIKit.h>
#import <CoreLocation/CoreLocation.h>

#ifndef MobSDK_Privacy_h
#define MobSDK_Privacy_h

//隐私数据配置代理
@protocol MOBFoundationPrivacyDelegate <NSObject>

@optional

/**
 用于判断是否允许SDK主动采集经纬度信息

 @return YES表示可以主动采集经纬度信息，NO表示不可以，默认为YES
 */
- (BOOL)isLocInfoEnable;

/**
 APP提供经纬度信息
 当SDK被拒绝主动采集经纬度信息后(isLocInfoEnable返回NO)，会通过此方法向App请求经纬度信息
 
 @return 经纬度信息,如果返回 nil 则表示不提供地理位置信息,默认为nil
 */
- (CLLocation* _Nullable)getLoc;

/**
 用于判断是否允许SDK主动采集wifi信息

 @return YES表示可以主动采集wifi信息，NO表示不可以，默认为YES
 */
- (BOOL)isWiFiInfoEnable;

/**
 APP提供wifi地址信息
 当SDK被拒绝主动采集wifi地址信息后(isWiFiInfoEnable返回NO)，会通过此方法向App请求wifi地址信息
 
 @return wifi名称信息,如果返回 nil 则表示不提供wifi地址信息,默认为nil
 */
- (NSString* _Nullable)getBSSID;

/**
 APP提供wifi名称信息
 当SDK被拒绝主动采集wifi名称信息后(isWiFiInfoEnable返回NO)，会通过此方法向App请求wifi名称信息
 
 @return wifi名称信息,如果返回 nil 则表示不提供wifi名称信息,默认为nil
 */
- (NSString* _Nullable)getSSID;

/**
 用于判断是否允许SDK主动采集idfv信息

 @return YES表示SDK可以主动采集idfv信息，NO表示不可以，默认为YES
 */
- (BOOL)isIdfvEnable;

/**
 APP提供idfv信息
 当SDK被拒绝主动采集idfv信息后(isIdfvEnable返回NO)，会通过此方法向App请求idfv信息
 
 @return idfa信息,如果返回 nil 则表示不提供idfv信息,默认为nil
 */
- (NSString* _Nullable)getIdfv;

/**
 用于判断是否允许SDK主动采集idfa信息

 @return YES表示SDK可以主动采集idfa信息，NO表示不可以，默认为YES
 */
- (BOOL)isIdfaEnable;

/**
 APP提供 idfa 信息
 当SDK被拒绝主动采集 idfa 信息后(isIdfaEnable返回NO)，会通过此方法向App请求idfa信息
 
 @return idfa信息,如果返回nil则表示不提供idfa信息,默认为nil
 */
- (NSString* _Nullable)getIdfa;

/**
 用于判断是否允许SDK主动采集ip信息
 
 @return YES表示SDK可以主动采集ip信息，NO表示不可以，默认为YES
 */
- (BOOL)isIpEnable;

/**
 APP提供 蜂窝 ipv4 信息
 当SDK被拒绝主动采集 蜂窝 ipv4 信息后(isIpEnable返回NO)，会通过此方法向App请求 蜂窝 ipv4 信息
 
 @return 蜂窝 ipv4信息,如果返回nil则表示不提供蜂窝ipv4信息,默认为nil
 */
- (NSString* _Nullable)getCellIpv4;

/**
 APP提供 蜂窝ipv6 信息
 当SDK被拒绝主动采集 蜂窝ipv6 信息后(isIpEnable返回NO)，会通过此方法向App请求蜂窝ipv6信息
 
 @return 蜂窝ipv6信息,如果返回nil则表示不提供蜂窝ipv6信息,默认为nil
 */
- (NSString* _Nullable)getCellIpv6;

/**
 APP提供 wifi ipv4 信息
 当SDK被拒绝主动采集 wifi ipv4 信息后(isIpEnable返回NO)，会通过此方法向App请求蜂窝wifi ipv4信息
 
 @return wifi ipv4 信息,如果返回nil则表示不提供 wifi ipv4 信息,默认为nil
 */
- (NSString* _Nullable)getWifiIpv4;

/**
 APP提供 wifi ipv6 信息
 当SDK被拒绝主动采集 wifi ipv6 信息后(isIpEnable返回NO)，会通过此方法向App请求蜂窝wifi ipv6信息
 
 @return wifi ipv6 信息,如果返回nil则表示不提供 wifi ipv6 信息,默认为nil
 */
- (NSString* _Nullable)getWifiIpv6;

/**
 APP提供 所有 ipv4, ipv6 信息
 当SDK被拒绝主动采集 ipv4, ipv6 信息后(isIpEnable返回NO)，会通过此方法向App请求 所有ipv4, ipv6信息
 
 @return ipv4, ipv6 信息,如果返回nil则表示不提供 ipv4, ipv6 信息,默认为nil
 */
- (NSDictionary* _Nullable)getIpInfoAll;


/**
 用于判断是否允许SDK主动采集社交平台信息

 @return YES表示SDK可以自行采集社交平台信息，NO表示不可以，默认为YES
 */
- (BOOL)isSocietyPlatformDataEnable;

- (BOOL)kbiAtAir;
@end


@interface MobSDK (Privacy)


/**
 获取MobTech用户隐私协议
 
 @param type 协议类型 (1= url类型, 2=  富文本类型)
 @param result 返回回调（data：字典类型 title=标题,content=内容(type=1，返回url,type = 2时返回富文本)       error:详细错误信息）
 */
+ (void)getPrivacyPolicy:(NSString * _Nullable)type
             compeletion:(void (^ _Nullable)(NSDictionary * _Nullable data,NSError * _Nullable error))result DEPRECATED_MSG_ATTRIBUTE("use -[getPrivacyPolicy:language:compeletion:] method instead.");

/**
 获取MobTech用户隐私协议
 
 @param type 协议类型 (1= url类型, 2=  富文本类型)
 @param language 隐私协议支持语言（）
 @param result 返回回调（data：字典类型 title=标题,content=内容(type=1，返回url,type = 2时返回富文本)       error:详细错误信息）
 */
+ (void)getPrivacyPolicy:(NSString * _Nullable)type
                language:(NSString * _Nullable)language
             compeletion:(void (^ _Nullable)(NSDictionary * _Nullable data,NSError * _Nullable error))result;

/**
 上传隐私协议授权状态
 @param isAgree 是否同意（用户授权后的结果）
 @param handler 回掉
 */
+ (void)uploadPrivacyPermissionStatus:(BOOL)isAgree
                             onResult:(void (^_Nullable)(BOOL success))handler;

/**
 上传隐私协议授权状态
 @param isAgree 是否同意（用户授权后的结果）
 @param privacyDataDelegate 隐私数据配置
 @param handler 回掉
 */
+ (void)uploadPrivacyPermissionStatus:(BOOL)isAgree
                  privacyDataDelegate:(id<MOBFoundationPrivacyDelegate> _Nullable)privacyDataDelegate
                             onResult:(void (^_Nullable)(BOOL success))handler;

/**
 设置隐私数据代理
 1.如果调用的uploadPrivacyPermissionStatus:privacyDataDelegate:onResult:中设置过privacyDataDelegate，
 就不用再调用setPrivacyDataDelegate:方法
 2.如果没有调用过uploadPrivacyPermissionStatus:privacyDataDelegate:onResult:.
 先调用setPrivacyDataDelegate:方法，再调uploadPrivacyPermissionStatus:onResult:
 3.也可以单独调用setPrivacyDataDelegate:方法

 @param privacyDataDelegate 隐私数据配置
 
 */
+(void)setPrivacyDataDelegate:(id<MOBFoundationPrivacyDelegate> _Nullable)privacyDataDelegate;

/**
 设置是否允许弹窗
 @param show 是否允许展示隐私协议二次弹窗（最好设置为YES，否则可能会导致MobTech部分功能无法使用，默认为YES）
 */
+ (void)setAllowShowPrivacyWindow:(BOOL)show  DEPRECATED_MSG_ATTRIBUTE("deprecated");

/**
 设置隐私协议弹窗色调
 @param backColor 弹窗背景色调
 @param colors 弹窗按钮色调数组（首个元素为拒绝按钮色调，第二个元素为同意按钮色调）
 */
+ (void)setPrivacyBackgroundColor:(UIColor *_Nullable)backColor
             operationButtonColor:(NSArray <UIColor *>*_Nullable)colors  DEPRECATED_MSG_ATTRIBUTE("deprecated");

@end


#endif /* MobSDK_Privacy_h */
