---
layout: docs
---

# @msrvida/sanddance

Visually explore, understand, and present your data.

![sanddance-animation](https://user-images.githubusercontent.com/11507384/54236654-52d42800-44d1-11e9-859e-6c5d297a46d2.gif)

[Demo](/app) - [API Reference](/docs/sanddance/v2/api)

## Installation

You will need to consider how to load the libraries that `sanddance` depends on, based on your build & deployment scenario.

## Installation via script tags

Load dependencies via `<script>` tags in your HTML:
```html
<script src="https://unpkg.com/vega@^5.8/build/vega.js" charset="utf-8"></script>
<script src="https://unpkg.com/deck.gl@~6.4/deckgl.min.js"></script>
<script src="https://unpkg.com/@msrvida/sanddance@^2/dist/umd/sanddance.js"></script>
```

A global variable named `SandDance` will be available to you. In your JavaScript, call the `use` function to pass the dependency libraries:

```js
SandDance.use(vega, deck, deck, luma);
```

## Installation via Node.js

Add these to the `dependencies` section of your `package.json`, then run `npm install`:

```json
"@deck.gl/core": "6.4",
"@deck.gl/layers": "6.4",
"@msrvida/sanddance": "^2",
"luma.gl": "6.4",
"vega": "^5.8"
```

Import these in your JavaScript, then call the `use()` function to pass the dependency libraries to `SandDance.VegaDeckGl`:

```js
import * as deck from '@deck.gl/core';
import * as layers from '@deck.gl/layers';
import * as luma from 'luma.gl';
import * as vega from 'vega';
import * as SandDance from '@msrvida/sanddance';

SandDance.use(vega, deck, layers, luma);
```

## For more information
Please visit the [SandDance website](/).
