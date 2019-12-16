/**
 * 深拷贝
 * @param {Array,Object} obj
 */
export const deepCopy = (obj) => {
  // 根据obj的类型判断是新建一个数组还是一个对象
  const newObj = obj instanceof Array ? [] : {}
  for (const key in obj) {
    // 判断属性值的类型，如果是对象递归调用深拷贝
    newObj[key] = typeof obj[key] === 'object' ? deepCopy(obj[key]) : obj[key]
  }
  return newObj
}
