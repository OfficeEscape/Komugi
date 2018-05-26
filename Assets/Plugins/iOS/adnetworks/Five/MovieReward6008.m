//
//  MovieReward6008.m(Five)
//
//  Copyright (c) A .D F. U. L. L. Y Co., Ltd. All rights reserved.
//
//

#import "MovieReward6008.h"
#import <FiveAd/FiveAd.h>

@interface MovieReward6008()<FADDelegate>

@property (nonatomic)FADFullScreen *fullscreen;
@property (nonatomic, strong)NSString *fiveAppId;
@property (nonatomic, strong)NSString *fiveSlotId;
@property (nonatomic, strong)NSString* submittedPackageName;
@property (nonatomic)BOOL testFlg;


@end

@implementation MovieReward6008

-(void)setData:(NSDictionary *)data
{
    self.fiveAppId = [NSString stringWithFormat:@"%@", [data objectForKey:@"app_id"]];
    self.fiveSlotId = [NSString stringWithFormat:@"%@", [data objectForKey:@"slot_id"]];
    self.testFlg = [[data objectForKey:@"test_flg"] boolValue];
    self.submittedPackageName = [data objectForKey:@"package_name"];
}

-(BOOL)isPrepared{
    //申請済のバンドルIDと異なる場合のメッセージ
    //(バンドルIDが申請済のものと異なると、正常に広告が返却されない可能性があります)
    if(![self.submittedPackageName isEqualToString:[[NSBundle mainBundle] bundleIdentifier]]) {
        //表示を消したい場合は、こちらをコメントアウトして下さい。
        NSLog(@"[ADF][Five]アプリのバンドルIDが、申請されたものと異なります。");
    }
    
    if (self.fullscreen) {
        return self.fullscreen.state == kFADStateLoaded;
    }
    return NO;
}

-(void)startAd
{
    if (self.fullscreen && self.fullscreen.state == kFADStateShowing) {
        return;
    }
    [MovieConfigure6008 configureWithAppId:self.fiveAppId isTest:self.testFlg];
    self.fullscreen = [[FADFullScreen alloc] initWithSlotId:self.fiveSlotId];
    self.fullscreen.delegate = self;
    
    [self.fullscreen loadAd];
}

-(void)showAd
{
    BOOL res = [self.fullscreen show];
    if (!res) {
        if (self.delegate && [self.delegate respondsToSelector:@selector(AdsPlayFailed:)]) {
            [self.delegate AdsPlayFailed:self];
        }
    }
}

-(void)showAdWithPresentingViewController:(UIViewController *)viewController
{
    [self showAd];
}

-(BOOL)isClassReference
{
    Class clazz = NSClassFromString(@"FADFullScreen");
    if (clazz) {
    } else {
        NSLog(@"Not found Class: FiveAd");
        return NO;
    }
    return YES;
}

@end

