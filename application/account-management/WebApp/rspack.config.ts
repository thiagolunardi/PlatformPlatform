import os from "os";
import { join, resolve } from "path";
import { Configuration, DefinePlugin } from "@rspack/core";
import HtmlWebpackPlugin from "html-webpack-plugin";
import HtmlWebpackHarddiskPlugin from "html-webpack-harddisk-plugin";

const buildEnv: BuildEnv = {};

const outputPath = resolve(__dirname, "dist");

const configuration: Configuration = {
  context: __dirname,
  entry: {
    main: ["./src/lib/rspack/runtime.ts", "./src/main.tsx"],
  },
  output: {
    path: outputPath,
    filename: process.env.NODE_ENV === "production" ? "[name].[contenthash].bundle.js" : undefined,
  },
  resolve: {
    tsConfigPath: resolve(__dirname, "tsconfig.json"),
  },
  module: {
    rules: [
      {
        test: /\.svg$/,
        type: "asset",
      },
      {
        test: /\.css$/,
        use: [
          {
            loader: "postcss-loader",
            options: {
              postcssOptions: {
                plugins: {
                  tailwindcss: {},
                  autoprefixer: {},
                },
              },
            },
          },
        ],
        type: "css",
      },
    ],
  },
  plugins: [
    // @ts-ignore
    new HtmlWebpackPlugin({
      template: "./public/index.html",
      meta: {
        runtimeEnv: "<ENCODED_RUNTIME_ENV>",
      },
      alwaysWriteToDisk: true,
      publicPath: "<CDN_URL>",
    }),
    new DefinePlugin({
      "import.meta.build_env": JSON.stringify(buildEnv),
      "import.meta.runtime_env": "getApplicationEnvironment().runtimeEnv",
      "import.meta.env": "getApplicationEnvironment().env",
    }),
    new HtmlWebpackHarddiskPlugin({
      outputPath,
    }),
  ],
  devServer: {
    allowedHosts: "all",
    headers: {
      "Access-Control-Allow-Origin": "*",
    },
    server: {
      type: "https",
      options: {
        pfx: join(os.homedir(), ".aspnet", "https", "localhost.pfx"),
        passphrase: process.env.CERTIFICATE_PASSWORD,
      },
    },
  },
};

export default configuration;
