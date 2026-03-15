import {resolve} from 'node:path';
import {defineConfig} from 'vite';
import {viteStaticCopy} from 'vite-plugin-static-copy';
import react from '@vitejs/plugin-react';

/*
      See https://vitejs.dev/config/
*/

export default defineConfig({
    plugins: [
        react(),
        viteStaticCopy({
            targets: [
                {
                    src: '../manifest.json',
                    dest: '.'
                },
                {
                    src: '*.*',
                    dest: '.'
                },
                {
                    src: '../public/*.*',
                    dest: '.'
                }
            ]
        }),
        viteStaticCopy({
            targets: [
                // Widget icons and configurations
                {
                    src: 'widgets/**/*.{svg,png,jpg,json}',
                    dest: '.'
                }
            ],
            structured: true
        })
    ],
    server: {
        allowedHosts: [
            'localhost',
            '127.0.0.1',
            '.ngrok-free.app'
        ]
    },
    root: './src',
    base: '',
    publicDir: 'public',
    build: {
        outDir: '../dist',
        emptyOutDir: true,
        copyPublicDir: false,
        target: ['es2022'],
        assetsDir: 'widgets/assets',
        rollupOptions: {
            onwarn(warning, warn) {
                if (warning.code === 'MODULE_LEVEL_DIRECTIVE' && warning.message.includes('react-virtualized')) {
                    return;
                }
                warn(warning);
            },
            input: {
                // List every widget entry point here
                userRating: resolve(__dirname, 'src/widgets/user-dashboard/index.html'),
                userCard: resolve(__dirname, 'src/widgets/user-card/index.html'),
                teamRating: resolve(__dirname, 'src/widgets/team-dashboard/index.html'),
                adminPanel: resolve(__dirname, 'src/widgets/admin-panel/index.html'),
                userProfile: resolve(__dirname, 'src/widgets/user-profile/index.html'),
            }
        }
    }
});
