---
title: 建立博客过程的记录
date: 2022-04-08T11:52:32.0000000
tags:
- 技术笔记
---



当我已经在Python的浩瀚大海遨（zheng）游（zha）了半个暑假后，我决定尝试一下传说中程序员专用的学(zhuang)习(bi)手(fangfa)段(fa)——建立自己的个人博客。作为一个半懂不懂的Python程序员，心中冒出的第一个想法自然是采用Python的Django作为开发自己的个人博客的手段。然而，在阅读了[用Django搭建个人博客](https://www.dusaiphoto.com/article/2/)等的其他人搭建这类动态博客的过程记录之后，我便义无反顾的转向了采用javascript开发的博客框架[Hexo](https://hexo.io)，<del>说好的Python信仰呢</del>。无他，唯简单尔。

<!--more-->

## 安装需要的程序

### 安装javascipt的运行环境

Hexo作为一个基于javasrcipt的博客框架，第一步自然是安装运行Javascript的环境。

Node.js就是几个基于Chrome V8引擎的Javascript运行时环境。这是一个异步事件驱动的Javascript运行时环境。同时Node还带有一个和`pip`功能类似的包管理工具`npm`,使我们可以方便的安装其他人开发的功能扩展包。我们就使用这种方便的方式安装`hexo`。

访问[node.js官方网站](https://nodejs.org/en/)下载了node.js的14.17.4 LTS版本的安装包，下载完成后安装。在Powershell中输入

```
node -v
```
若能显示出node的版本
```
v14.17.4
```
便说明node.js安装成功。
再输入
```
npm -v
```
显示出npm的版本
```
7.20.3
```
便说明npm也安装完成。
>这里要特别说明，npm的更新快于node，可能在不久之后npm的版本就不是7.20.3了
>
>同时可以使用`npm install -g npm`方便的升级npm

### 安装Hexo

在Power shell中输入
```
npm install -g hexo-cli
```
>这里的-g代表global，即为全局安装Hexo，如果像我一样初次使用npm建议安装所有的包时都加上-g。

npm的默认安装源在国外，如果在安装过程中遇到网络问题，可以像我们使用其他的包管理器一样换为国内源进行安装。在国内比较出名的npm镜像源是由部分淘宝程序员维护的淘宝源。

> 2022年，淘宝源的域名由https://npm.taobao.org更改为https://npmmirror.com。

#### 临时使用淘宝源作为下载方式

在利用npm安装npm包时，使用
```
npm --registry https://registry.npmmirror.com install 
```
可以在本次下载包时采用淘宝源作为下载地址。

#### 将下载地址设置为淘宝源

```
npm config set registry  https://registry.npmmirror.com
```
这样设置以后，每次下载包时都会从淘宝的服务器下载。

#### 使用cnpm下载

cnpm是国内一些热心于开源的程序眼开发的一个node包，作用和node.js自带的npm完全一样，不过默认使用淘宝源下载。在下载cnpm之后就可以方便的在国内源和国外源之间切换，当使用国内源时使用
```bash
cnpm install 
```
使用国外源时使用
```
npm install 
```
这就避免了在需要使用国外源时来回切换的麻烦。<del>虽然我应该不会用到国外源</del>

开始时使用

```
npm install cnpm -g
```
安装cnpm
然后使用

```
cnpm install Hexo-cli -g
```
安装Hexo博客框架。
再输入
```
hexo -v
```
验证安装是否完成。

我这里的输出是

```
hexo-cli: 4.3.0
os: win32 10.0.19043
node: 14.17.4
v8: 8.4.371.23-node.76
uv: 1.41.0
zlib: 1.2.11
brotli: 1.0.9
ares: 1.17.1
modules: 83
nghttp2: 1.42.0
napi: 8
llhttp: 2.1.3
openssl: 1.1.1k
cldr: 39.0
icu: 69.1
tz: 2021a
unicode: 13.0
```

## 初次使用Hexo

### 创建博客

进入一个我们准备用来设置博客的文件夹，在终端中输入
```
Hexo init blog
```
Hexo会以blog为名称创建一个博客文件夹，这个文件夹的内容为

![文件夹截图](1.webp)

`node_modules`文件夹是Hexo需要用到的一些npm依赖包的存放地址，`public`文件夹下是由Hexo渲染产生的静态博客文件，`scaffolds`文件夹是博客用到的模板文件，在默认情况下应该有`draft.md`,`page.md`,`post.md`三个模板文件。`themes`是Hexo中可以使用的主题文件。主题也是Hexo一个非常方便的设计，我们可以方便使用其他人编写的Hexo Themes，让自己的博客在不同的风格之间变换。`source`文件夹就是存放我们写作的博客的地方。一般这里面会有两个子文件夹，`_draft`, `_posts`。我们在里面在创建一个`img`文件夹，把自己的头像图片和网站的图标文件都放在里面，在之后的设置的时候使用。

在终端中输入

```bash
❯ hexo server
INFO  Validating config
INFO  Start processing
INFO  Hexo is running at http://localhost:4000/ . Press Ctrl+C to stop.
```

会在本地运行Hexo自带的一台静态博客服务器。我们用浏览器访问http://localhost:4000, 就可以看见Hexo博客的初始界面

![初始截图](2.webp)

这便说明安装成功了，~~可以开香槟了~~

### 写作

在终端中输入

```bash
hexo new "文章标题"
```

> 我已经在设置中设置，不指定模板是自动生成草稿。具体设置见下一节。

然后就会在前面提到的`_draft`文件夹下创建一个markdown文件和一个同名的资源文件夹，在资源文件夹下放置在文章中会用到的图片。接下来使用一款适合自己的markdown编辑器就可以开始文章的写作了。

> 我使用的markdown编辑器是[Typora](https://typora.io/),但是这个软件在更新到正式版之后就开始收费了，不过我们可以在[这里](https://typora.io/releases/all)找到版本小于1.0.0的beta版本使用。

为了方便的在写作时插入图片，我下载安装了`hexo-asset-image`这个Hexo插件，但是由于这个插件总是在我的电脑上犯病，我自己做了一点修改，放在了[我的github上](https://github.com/jackfiled/hexo-asset-image)。

通过

```bash
npm install git+https://github.com/jackfiled/hexo-asset-image --save
```

来安装我修改之后的包。

这样在typora中写作的时候，先通过格式-图像-设置图片根目录为hexo自动生成的资源文件夹，在需要插入图片时通过

```markdown
![example](example.png)
```

这种方式来插入图片，其中`example.png`图片在资源文件夹下。这样在typora中可以正确显示，在hexo渲染出来的网页中也可以正确的显示。

在完成写作之后，使用

```bash
hexo publish "文章标题"
```

将文章发布，把markdown文件和资源文件夹从`_draft`文件夹移动到`_post`文件夹。

这时使用

```
hexo server
```

就可以看见我们完成的博客文章了。

### 设置主题

为了选择一个恰当而合适的主题，以期避免可能引起的一些不必要的误会和不便，我们设立一个跨部门的多方委员会，用以充分考虑各方的意见，同时选择专门的专家主持流程严谨的研究，充分考虑科学界的意见。通过多次多方的协调会议，在完全理解各方需求的之后，委员会提供了多种选择以供我们选择。

> 汉弗莱附身了属于是

简而言之，我看了几个博客，下载了几个主题测试，然后选择了[yilia-plus](https://github.com/JoeyBling/hexo-theme-yilia-plus)。我比较看重的这个主题的原因是他比较简洁。

在blog文件夹中的themes文件夹`git clone`我们选择好的主题，在blog文件夹下的`_config.yaml`中设置主题

```yaml
theme: yilia-plus
```

然后我们在运行

```bash
hexo server
```

就可以看见我们的主题设置已经生效。

### 调整设置文件

Hexo在一般情况下有两个配置文件我们会经常用到，blog根目录下的`_config.yaml`, 我们下载的主题文件夹下的`_config.yaml`

#### Hexo的设置

```yaml
# Site
title: Ricardo的博客
subtitle: '奇奇怪怪东西的聚居地'
description: ''
keywords:
author: Ricardo Ren
language: zh-CN
timezone: ''
```

第一部分的网站设置部分，根据自己的需求修改

```yaml
# URL
## Set your site url here. For example, if you use GitHub Page, set url as 'https://username.github.io/project'
url: http://rrricardo.top/blog
permalink: :year/:month/:day/:title/
permalink_defaults:
pretty_urls:
  trailing_index: true # Set to false to remove trailing 'index.html' from permalinks
  trailing_html: true # Set to false to remove trailing '.html' from permalinks
```

第二部分，在url的地方填写自己的博客的地址。

第三部分`Directory`中我没有修改任何玩意儿。

第四部分`Writing`中我把默认使用的模板设置为草稿, 再将`post_asset_folder`设置为true,这样在使用`hexo new `命令使就会再md文件所在的目录创建一个同名的资源文件夹，把我们文章中会使用到的图片放在里面。

```yaml
default_layout: draft
post_asset_folder: true
```

剩下的大部分我们就可以不用修改了。

#### yilia-plus的设置

> 由于这个主题是国人开发的，配置文件都有详细的中文注释，按着注释走就完事儿了。

## 将博客部署到云服务器

### 设置deploy

Hexo做为一个静态的博客框架，可以将整个博客网站直接渲染为静态页面,我们可以执行

```
hexo g
```

Hexo就会在`public`文件夹下生成整个博客的静态界面，我们只用在服务器上放置这些文件就可以了。

这里使用hexo提供的deploy功能来简化本地同步到git仓库的过程。

在终端执行下列命令，安装git部署插件

```bash
npm install hexo-deployer-git --save
```

在根目录下的`_config.yaml`中设置

```yaml
# Deployment
## Docs: https://hexo.io/docs/one-command-deployment
deploy:
  type: git
  repo: git@gitee.com:ricardo-ren/blog-deploy.git
  branch: master
  message: Site Update {{now('YYYY-MM-DD HH:mm:ss')}}
```

设置中的message信息可以按自己的喜好设置。

> 如果使用这种方式记得先在git上创建远程仓库

设置完成后执行

```bash
hexo deploy
```

可能会提示没有设置用户名和邮箱

```bash
cd .deploy_git
git config ...
```

这里进入的`.deploy_git`实际上就是git仓库在本地的位置。

### 服务器clone

在服务器上适当的位置执行

```bash 
git clone git@gitee.com:ricardo-ren/blog-deploy.git
```

这里将作为博客网站的根目录。

### nginx设置

在云服务器上我使用nginx作为反向代理服务器。由于nginx也是一个不错的静态资源服务器，hexo博客也就用nginx作为服务器了。

首先安装nginx

```bash
sudo apt install nginx
```

然后编写nginx的配置文件，在`/etc/nginx/nginx.conf`

```nginx
user root;
worker_processes auto;
pid /run/nginx.pid;
include /etc/nginx/modules-enabled/*.conf;
events {
        worker_connections 768;
        # multi_accept on;
}

http {

        ##
        # Basic Settings
        ##

        sendfile on;
        tcp_nopush on;
        tcp_nodelay on;
        keepalive_timeout 65;
        types_hash_max_size 2048;
        # server_tokens off;

        # server_names_hash_bucket_size 64;
        # server_name_in_redirect off;

        include /etc/nginx/mime.types;
        default_type application/octet-stream;

        ##
        # SSL Settings
        ##

        ssl_protocols TLSv1 TLSv1.1 TLSv1.2 TLSv1.3; # Dropping SSLv3, ref: POODLE
        ssl_prefer_server_ciphers on;

        ##
        # Logging Settings
        ##

        access_log /var/log/nginx/access.log;
        error_log /var/log/nginx/error.log;

        ##
        # Gzip Settings
        ##

        gzip on;

        # gzip_vary on;
        # gzip_proxied any;
        # gzip_comp_level 6;
        # gzip_buffers 16 8k;
        # gzip_http_version 1.1;
        # gzip_types text/plain text/css application/json application/javascript text/xml application/xml application/xml+rss text/javascript;

        ##
        # Virtual Host Configs
        ##
		
		#这两行我注释了，否则配置文件貌似不会生效
        #include /etc/nginx/conf.d/*.conf;
        #include /etc/nginx/sites-enabled/*;

        # server
        server {
                listen 443 ssl;
                server_name rrricardo.top;

                # ssl settings
                ssl_certificate /etc/letsencrypt/live/rrricardo.top/fullchain.pem;
                ssl_certificate_key /etc/letsencrypt/live/rrricardo.top/privkey.pem;

                include /etc/letsencrypt/options-ssl-nginx.conf;
                ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem;


                location / {
                        return 301 https://rrricardo.top/blog/;
                }

                location /blog/ {
                        root /home/rcj/website/;
                        index index.html index.htm;
                }
        }

        server {
                listen 80;
                server_name rrricardo.top;

                return 301 https://$server_name$request_uri;
        }
}
```

nginx大部分的默认设置都没有改动，指设置了Let's Encrypt提供的HTTPS证书以提供HTTPS服务，博客网站挂载在443端口的`/blog/`下，当访问443端口的`/`时会301重定向到`/blog/`上。

> 安装Let's Encrypt的服务主要参考[Let's Encrypt](https://letsencrypt.org/zh-cn/getting-started/),[Certbot Instructions | Certbot (eff.org)](https://certbot.eff.org/instructions?ws=nginx&os=ubuntufocal),还有[免费 https 证书（Let's Encrypt）申请与配置 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/21286171)。

### 网站备案

按照你的云服务器提供商的指南进行就可以了，我的备案过程还算比较顺利。

## 后记

从2021年9月15日博客仓库的首次提交到这篇博客完成，已经过去了七个月的时间，经过七个月不断的修补和改进，我的博客终于也算是有了一个博客的样子。

一路上读了许多人的博客，已经无法一一指出，在此一并表示感谢。

文中也不免有许多疏漏之处，因时间飞逝，当时遇到的一些问题也无法一一记录，还请诸位读者海涵。	
