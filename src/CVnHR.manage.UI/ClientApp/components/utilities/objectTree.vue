<template>
  <div>
    <div v-if="item">
      <div v-for="(value, label) in item">
        <objectTree :label="label" :value="value" :depth="0"></objectTree>
      </div>
    </div>
    <div v-else-if="label" :style="indent">
      
      <label @click="toggleChildren" :class="collapser">
        <icon v-if="isObject(value)" :icon="iconClasses" />
        {{ label }}:
        <span v-if="!isObject(value)">
          {{ formatValue }}
        </span>
      </label>
      <template v-if="showChildren && isObject(value)">
        <template v-if="!isArray(value)">
          <objectTree v-for="(val, label) in value"
                      :label="label"
                      :value="val"
                      :key="label + depth"
                      :depth="depth + 1"></objectTree>
        </template>
        <template v-else>
          <template v-for="(val, index) in value">
            [
            <template v-if="isObject(val)">
              <objectTree v-for="(val, label) in val"
                          :label="label"
                          :value="val"
                          :key="label + depth + index"
                          :depth="depth + 1"></objectTree>
            </template>
            <template v-else>
              <span>"{{val}}"</span>
            </template>
            ]
            <span v-if="index != Object.keys(value).length - 1">,</span>
          </template>
        </template>
      </template>
    </div>
  </div>
</template>

<script>
  export default {
    props: ['label', 'value', 'depth', 'item'], 
    name: 'objectTree',
    data() {
      return { showChildren: true } // TODO: make collapse all possible (state?)
    },

    computed: {
      indent() {
        return this.depth > 0 ? { transform: `translate(25px)` } : null
      },
      collapser() {
        return this.isObject(this.value) ? 'collapser' : null;
      },
      
      formatValue() {
        return this.value ? this.value : this.value == null ? '[null]' : '[empty]';
      },
      iconClasses() {
        return this.showChildren ? ['far', 'minus-square'] : ['far', 'plus-square']
      }
    },

    methods: {
      isObject(value) {
        return value && typeof value === 'object';
      },
      isArray(value) {
        return this.isObject(value) && value.constructor === Array;
      },
      toggleChildren() {
        this.showChildren = !this.showChildren;
      }
    }
  }
</script>

<style scoped>
  label {
    font-weight: bold;
    margin: 0
  }
    label span {
      font-weight: normal;
    }

  .collapser {
    cursor: pointer;
  }
</style>
