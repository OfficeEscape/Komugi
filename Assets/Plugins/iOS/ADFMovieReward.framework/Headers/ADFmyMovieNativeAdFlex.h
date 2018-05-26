//
//  ADFmyMovieNativeAdFlex.h
//  ADFMovieReward
//
//  Created by Junhua Li on 2017/12/28.
//  Copyright © 2017年 A .D F. U. L. L. Y Co., Ltd. All rights reserved.
//

#import "ADFmyMovieReward.h"

@interface ADFmyMovieNativeAdFlex : NSObject

/**
 動画ネイティブ広告（FLEX）のサポート対象OSバージョンかどうかを返却します

 @return YES: サポート対象OSバージョン, No: サボート対象外
 */
+ (BOOL)isSupportedOSVersion;

/**
 動画ネイティブ広告（FLEX）を初期化します

 @param appID 広告枠ID
 @param viewController 広告を表示するViewController
 */
+ (void)initWithAppID:(NSString *)appID viewController:(UIViewController*)viewController;

/**
 動画ネイティブ広告（FLEX）のデリゲートの設定とインスタンスを返却します

 @param appID 広告枠ID
 @param delegate デリゲートオブジェクト
 @return 動画ネイティブ広告（FLEX）のインスタンス
 */
+ (ADFmyMovieNativeAdFlex *)getInstance:(NSString *)appID delegate:(id<ADFmyMovieRewardDelegate>)delegate;

/**
 動画ネイティブ広告（FLEX）の準備状況を返却します

 @return YES: 表示可能, NO: 準備中
 */
- (BOOL)isPrepared;

/**
 動画ネイティブ広告（FLEX）の表示を開始します
 */
- (void)play;
- (void)playWithPresentingViewController:(UIViewController *)viewController;

/**
 動画ネイティブ広告（FLEX）の表示を終了します
 画面遷移など任意のタイミングで広告を終了したい場合に利用してください
 */
- (void)finish;

@end
