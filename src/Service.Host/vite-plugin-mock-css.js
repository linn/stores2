export default function mockCssPlugin() {
    return {
        name: 'mock-css',
        transform(code, id) {
            if (id.endsWith('.css')) {
                return 'export default {}';
            }
        }
    };
}
